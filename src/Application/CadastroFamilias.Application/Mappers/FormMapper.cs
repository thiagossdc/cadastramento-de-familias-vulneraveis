using CadastroFamilias.Application.DTOs;
using CadastroFamilias.Core.Entities;

namespace CadastroFamilias.Application.Mappers;

/// <summary>
/// Mapeador entre Entidades de Domínio e DTOs
/// Separação entre camadas - Não expõe entidades diretamente na API
/// </summary>
public static class FormMapper
{
    public static Form ToDomain(this CreateFormRequest request)
    {
        return new Form(
            cadastralData: request.CadastralData.ToDomain(),
            familyMembers: request.FamilyMembers.Select(m => m.ToDomain()),
            educationData: request.EducationData.ToDomain(),
            familyNeeds: request.FamilyNeeds,
            essentialItems: request.EssentialItems,
            healthNeeds: request.HealthNeeds,
            hasProfessionalSkills: request.HasProfessionalSkills,
            occupation: request.Occupation,
            experienceTime: request.ExperienceTime,
            occupationOther: request.OccupationOther,
            skills: request.Skills,
            socialIssues: request.SocialIssues,
            otherIssues: request.OtherIssues,
            incomeExpensesData: request.IncomeExpensesData.ToDomain(),
            housingData: request.HousingData.ToDomain());
    }

    public static FormResponse ToResponse(this Form form)
    {
        return new FormResponse(
            Id: form.Id,
            CadastralData: form.CadastralData.ToDto(),
            FamilyMembers: form.FamilyMembers.Select(m => m.ToDto()),
            EducationData: form.EducationData.ToDto(),
            FamilyNeeds: form.FamilyNeeds,
            EssentialItems: form.EssentialItems,
            HealthNeeds: form.HealthNeeds,
            HasProfessionalSkills: form.HasProfessionalSkills,
            Occupation: form.Occupation,
            ExperienceTime: form.ExperienceTime,
            OccupationOther: form.OccupationOther,
            Skills: form.Skills,
            SocialIssues: form.SocialIssues,
            OtherIssues: form.OtherIssues,
            IncomeExpensesData: form.IncomeExpensesData.ToDto(),
            HousingData: form.HousingData.ToDto(),
            CreatedAt: form.CreatedAt,
            UpdatedAt: form.UpdatedAt);
    }

    #region Mapeamentos Value Objects

    public static CadastralData ToDomain(this CadastralDataDto dto)
        => new(dto.ApplicantName, dto.Cpf, dto.Rg, dto.BirthDate, dto.MaritalStatus,
               dto.Address, dto.Number, dto.Neighborhood, dto.City, dto.State, dto.Cep,
               dto.Phone, dto.Email, dto.FamilyMembersCount);

    public static CadastralDataDto ToDto(this CadastralData domain)
        => new(domain.ApplicantName, domain.Cpf, domain.Rg, domain.BirthDate, domain.MaritalStatus,
               domain.Address, domain.Number, domain.Neighborhood, domain.City, domain.State, domain.Cep,
               domain.Phone, domain.Email, domain.FamilyMembersCount);

    public static FamilyMember ToDomain(this FamilyMemberDto dto)
        => new(dto.Name, dto.Age, dto.Relationship, dto.Occupation, dto.Schooling);

    public static FamilyMemberDto ToDto(this FamilyMember domain)
        => new(domain.Name, domain.Age, domain.Relationship, domain.Occupation, domain.Schooling);

    public static EducationData ToDomain(this EducationDataDto dto)
        => new(dto.SchoolingLevel, dto.IsAttendingSchool, dto.Course, dto.HasProfessionalTraining, dto.TrainingArea);

    public static EducationDataDto ToDto(this EducationData domain)
        => new(domain.SchoolingLevel, domain.IsAttendingSchool, domain.Course, domain.HasProfessionalTraining, domain.TrainingArea);

    public static IncomeExpensesData ToDomain(this IncomeExpensesDataDto dto)
        => new(dto.MonthlyFamilyIncome, dto.TotalExpenses, dto.RentValue, dto.ReceivesGovernmentBenefit, dto.BenefitType, dto.BenefitValue);

    public static IncomeExpensesDataDto ToDto(this IncomeExpensesData domain)
        => new(domain.MonthlyFamilyIncome, domain.TotalExpenses, domain.RentValue, domain.ReceivesGovernmentBenefit, domain.BenefitType, domain.BenefitValue);

    public static HousingData ToDomain(this HousingDataDto dto)
        => new(dto.HousingType, dto.OwnProperty, dto.RoomsCount, dto.BedroomsCount, dto.HasBasicSanitation, dto.HasElectricity, dto.HasRunningWater);

    public static HousingDataDto ToDto(this HousingData domain)
        => new(domain.HousingType, domain.OwnProperty, domain.RoomsCount, domain.BedroomsCount, domain.HasBasicSanitation, domain.HasElectricity, domain.HasRunningWater);

    #endregion
}