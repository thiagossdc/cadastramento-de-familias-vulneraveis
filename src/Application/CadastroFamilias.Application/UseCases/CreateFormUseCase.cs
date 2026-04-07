using CadastroFamilias.Application.DTOs;
using CadastroFamilias.Application.Mappers;
using CadastroFamilias.Core.Interfaces;
using MassTransit;
using MediatR;

namespace CadastroFamilias.Application.UseCases;

/// <summary>
/// Caso de Uso para Criação de Formulário
/// Implementa CQRS e Separação de Responsabilidades
/// </summary>
public class CreateFormUseCase : IRequestHandler<CreateFormRequest, FormResponse>
{
    private readonly IFormRepository _formRepository;

    public CreateFormUseCase(IFormRepository formRepository)
    {
        _formRepository = formRepository;
    }

    public async Task<FormResponse> Handle(CreateFormRequest request, CancellationToken cancellationToken)
    {
        // 1. Converter DTO para Entidade de Domínio
        var form = request.ToDomain();
        
        // 2. Validar regras de negócio
        Validate(form);
        
        // 3. Persistir no banco de dados
        var createdForm = await _formRepository.CreateAsync(form, cancellationToken);
        
        // 4. Evento RabbitMQ pode ser adicionado depois
        // await _publishEndpoint.Publish(new FormCreatedEvent
        // {
        //     FormId = createdForm.Id,
        //     ApplicantName = createdForm.CadastralData.ApplicantName,
        //     CreatedAt = createdForm.CreatedAt
        // }, cancellationToken);
        
        // 5. Converter para DTO de resposta
        return createdForm.ToResponse();
    }

    private static void Validate(Core.Entities.Form form)
    {
        // Aplicar regras de negócio aqui
        if (form.CadastralData.FamilyMembersCount < 0)
            throw new InvalidOperationException("Quantidade de membros da família não pode ser negativa");
            
        if (form.IncomeExpensesData.MonthlyFamilyIncome < 0)
            throw new InvalidOperationException("Renda familiar não pode ser negativa");
    }
}

/// <summary>
/// Evento publicado no RabbitMQ quando um formulário é criado
/// </summary>
public record FormCreatedEvent
{
    public int FormId { get; init; }
    public string ApplicantName { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
}