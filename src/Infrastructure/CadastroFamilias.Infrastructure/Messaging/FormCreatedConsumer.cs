using CadastroFamilias.Application.UseCases;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace CadastroFamilias.Infrastructure.Messaging;

/// <summary>
/// Consumer do RabbitMQ para processamento assíncrono de formulários criados
/// Implementa padrão Observer e Event Driven Architecture
/// </summary>
public class FormCreatedConsumer : IConsumer<FormCreatedEvent>
{
    private readonly ILogger<FormCreatedConsumer> _logger;

    public FormCreatedConsumer(ILogger<FormCreatedConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<FormCreatedEvent> context)
    {
        var @event = context.Message;
        
        _logger.LogInformation("📩 Processando formulário criado: Id={FormId}, Nome={ApplicantName}", 
            @event.FormId, @event.ApplicantName);
        
        try
        {
            // TODO: Implementar processamento assíncrono:
            // - Enviar email de confirmação
            // - Gerar relatório PDF
            // - Integrar com sistemas externos
            // - Calcular índice de vulnerabilidade
            // - Enviar notificações
            
            await Task.Delay(1000); // Simula processamento
            
            _logger.LogInformation("✅ Formulário Id={FormId} processado com sucesso!", @event.FormId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Erro ao processar formulário Id={FormId}", @event.FormId);
            throw; // RabbitMQ irá reprocessar automaticamente
        }
    }
}