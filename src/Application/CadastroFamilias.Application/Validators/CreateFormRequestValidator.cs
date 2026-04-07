using CadastroFamilias.Application.DTOs;
using FluentValidation;

namespace CadastroFamilias.Application.Validators;

public class CreateFormRequestValidator : AbstractValidator<CreateFormRequest>
{
    public CreateFormRequestValidator()
    {
        // Dados Cadastrais
        RuleFor(x => x.CadastralData.ApplicantName)
            .NotEmpty().WithMessage("Nome do solicitante é obrigatório")
            .MinimumLength(3).WithMessage("Nome deve ter pelo menos 3 caracteres")
            .MaximumLength(150).WithMessage("Nome não pode ter mais que 150 caracteres");

        RuleFor(x => x.CadastralData.Cpf)
            .NotEmpty().WithMessage("CPF é obrigatório")
            .Matches(@"^\d{11}$").WithMessage("CPF deve conter exatamente 11 dígitos numéricos");

        RuleFor(x => x.CadastralData.BirthDate)
            .NotEmpty().WithMessage("Data de nascimento é obrigatória")
            .LessThan(DateTime.Now).WithMessage("Data de nascimento deve ser menor que a data atual");

        RuleFor(x => x.CadastralData.Email)
            .EmailAddress().WithMessage("Email inválido")
            .When(x => !string.IsNullOrEmpty(x.CadastralData.Email));

        RuleFor(x => x.CadastralData.Phone)
            .Matches(@"^\d{10,11}$").WithMessage("Telefone deve ter 10 ou 11 dígitos")
            .When(x => !string.IsNullOrEmpty(x.CadastralData.Phone));

        RuleFor(x => x.CadastralData.Cep)
            .Matches(@"^\d{8}$").WithMessage("CEP deve conter exatamente 8 dígitos")
            .When(x => !string.IsNullOrEmpty(x.CadastralData.Cep));

        // Composição Familiar
        RuleFor(x => x.FamilyMembers)
            .NotNull().WithMessage("Lista de membros da família é obrigatória");

        // Financeiro
        RuleFor(x => x.IncomeExpensesData.MonthlyFamilyIncome)
            .GreaterThanOrEqualTo(0).WithMessage("Renda familiar mensal não pode ser negativa");

        RuleFor(x => x.IncomeExpensesData.TotalExpenses)
            .GreaterThanOrEqualTo(0).WithMessage("Total de despesas não pode ser negativo");
    }
}