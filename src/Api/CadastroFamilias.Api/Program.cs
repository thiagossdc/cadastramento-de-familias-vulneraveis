using CadastroFamilias.Api.Middleware;
using CadastroFamilias.Application.UseCases;
using CadastroFamilias.Application.Validators;
using CadastroFamilias.Core.Interfaces;
using CadastroFamilias.Infrastructure.Persistence;
using CadastroFamilias.Infrastructure.Persistence.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Threading.RateLimiting;

// Carregar variáveis de ambiente do arquivo .env
DotNetEnv.Env.TraversePath().Load();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar CORS
builder.Services.AddCors(options =>
{
    var allowedOrigins = Environment.GetEnvironmentVariable("CORS_ALLOWED_ORIGINS")
                          ?.Split(',', StringSplitOptions.RemoveEmptyEntries)
                          ?? ["http://localhost:5173"];

    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Configurar Banco de Dados SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") 
                            ?? builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlite(connectionString);
});

// Desabilitado temporariamente - MassTransit 9+ requer licença comercial
// Para usar RabbitMQ, instale a versão 8.2.5 Open Source
// // Configurar MassTransit + RabbitMQ
// builder.Services.AddMassTransit(busConfigurator =>
// {
//     busConfigurator.SetKebabCaseEndpointNameFormatter();
//     
//     // Consumers
//     busConfigurator.AddConsumer<FormCreatedConsumer>();
//     
//     busConfigurator.UsingRabbitMq((context, configurator) =>
//     {
//         configurator.Host(new Uri(builder.Configuration["MessageBroker:Host"]!), h =>
//         {
//             h.Username(builder.Configuration["MessageBroker:Username"]!);
//             h.Password(builder.Configuration["MessageBroker:Password"]!);
//         });
//         
//         configurator.ConfigureEndpoints(context);
//     });
// });

// Injeção de Dependência - Repositórios
builder.Services.AddScoped<IFormRepository, FormRepository>();

// Validação FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<CreateFormRequestValidator>();

// Tratamento Global de Exceções
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Serilog - Logging Estruturado
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Rate Limiting
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    
    options.AddFixedWindowLimiter("FixedPolicy", limiterOptions =>
    {
        limiterOptions.PermitLimit = 100;
        limiterOptions.Window = TimeSpan.FromMinutes(1);
        limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        limiterOptions.QueueLimit = 0;
        limiterOptions.AutoReplenishment = true;
    });
});

// Versionamento de API
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// Compressão de Respostas
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
});

// Health Checks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<AppDbContext>();

// Injeção de Dependência - Casos de Uso
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateFormUseCase).Assembly));
// builder.Services.AddScoped<GenerateReportUseCase>();

var app = builder.Build();

// Tratamento Global de Exceções
app.UseExceptionHandler();

app.UseResponseCompression();
app.UseRateLimiter();

// Apply migrations automatically in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.DisplayRequestDuration();
        options.EnableDeepLinking();
        options.EnableTryItOutByDefault();
    });
    
    // Aplicar migrações automaticamente
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await dbContext.Database.MigrateAsync();
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapControllers().RequireRateLimiting("FixedPolicy");

app.MapHealthChecks("/health");

app.Run();
