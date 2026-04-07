using MediatR;

namespace CadastroFamilias.Application.DTOs;

/// <summary>
/// DTO para criação de novo Formulário
/// Implementa IRequest do MediatR para CQRS
/// </summary>
public record CreateFormRequest : IRequest<FormResponse>
{
    // Dados Cadastrais
    public CadastralDataDto CadastralData { get; set; } = null!;
    public IEnumerable<FamilyMemberDto> FamilyMembers { get; set; } = [];
    public EducationDataDto EducationData { get; set; } = null!;
    public IEnumerable<string> FamilyNeeds { get; set; } = [];
    public IEnumerable<string> EssentialItems { get; set; } = [];
    public string HealthNeeds { get; set; } = string.Empty;
    
    // Habilidades Profissionais
    public bool HasProfessionalSkills { get; set; }
    public string Occupation { get; set; } = string.Empty;
    public string ExperienceTime { get; set; } = string.Empty;
    public string OccupationOther { get; set; } = string.Empty;
    public string Skills { get; set; } = string.Empty;
    
    // Problemas Sociais
    public IEnumerable<string> SocialIssues { get; set; } = [];
    public string OtherIssues { get; set; } = string.Empty;
    
    // Dados Financeiros
    public IncomeExpensesDataDto IncomeExpensesData { get; set; } = null!;
    
    // Dados Habitacionais
    public HousingDataDto HousingData { get; set; } = null!;
}

public record CadastralDataDto(
    string ApplicantName,
    string Cpf,
    string Rg,
    DateTime BirthDate,
    string MaritalStatus,
    string Address,
    string Number,
    string Neighborhood,
    string City,
    string State,
    string Cep,
    string Phone,
    string Email,
    int FamilyMembersCount);

public record FamilyMemberDto(
    string Name,
    int Age,
    string Relationship,
    string Occupation,
    string Schooling);

public record EducationDataDto(
    string SchoolingLevel,
    bool IsAttendingSchool,
    string Course,
    bool HasProfessionalTraining,
    string TrainingArea);

public record IncomeExpensesDataDto(
    decimal MonthlyFamilyIncome,
    decimal TotalExpenses,
    decimal RentValue,
    bool ReceivesGovernmentBenefit,
    string BenefitType,
    decimal BenefitValue);

public record HousingDataDto(
    string HousingType,
    bool OwnProperty,
    int RoomsCount,
    int BedroomsCount,
    bool HasBasicSanitation,
    bool HasElectricity,
    bool HasRunningWater);

public record FormResponse(
    int Id,
    CadastralDataDto CadastralData,
    IEnumerable<FamilyMemberDto> FamilyMembers,
    EducationDataDto EducationData,
    IEnumerable<string> FamilyNeeds,
    IEnumerable<string> EssentialItems,
    string HealthNeeds,
    bool HasProfessionalSkills,
    string Occupation,
    string ExperienceTime,
    string OccupationOther,
    string Skills,
    IEnumerable<string> SocialIssues,
    string OtherIssues,
    IncomeExpensesDataDto IncomeExpensesData,
    HousingDataDto HousingData,
    DateTime CreatedAt,
    DateTime UpdatedAt);