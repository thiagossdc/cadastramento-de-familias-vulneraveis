using System;
using System.Collections.Generic;

namespace CadastroFamilias.Core.Entities
{
    /// <summary>
    /// Entidade principal que representa um cadastro social
    /// Segue os princípios do DDD e Arquitetura Hexagonal
    /// </summary>
    public class Form
    {
        public int Id { get; private set; }
        
        // Dados Cadastrais
        public CadastralData CadastralData { get; private set; }
        
        // Composição Familiar
        public IReadOnlyCollection<FamilyMember> FamilyMembers { get; private set; }
        
        // Dados Educacionais
        public EducationData EducationData { get; private set; }
        
        // Necessidades da Família
        public IReadOnlyCollection<string> FamilyNeeds { get; private set; }
        
        // Itens Essenciais necessários
        public IReadOnlyCollection<string> EssentialItems { get; private set; }
        
        // Necessidades de Saúde
        public string HealthNeeds { get; private set; }
        
        // Habilidades Profissionais
        public bool HasProfessionalSkills { get; private set; }
        public string Occupation { get; private set; }
        public string ExperienceTime { get; private set; }
        public string OccupationOther { get; private set; }
        public string Skills { get; private set; }
        
        // Problemas Sociais
        public IReadOnlyCollection<string> SocialIssues { get; private set; }
        public string OtherIssues { get; private set; }
        
        // Dados Financeiros
        public IncomeExpensesData IncomeExpensesData { get; private set; }
        
        // Dados Habitacionais
        public HousingData HousingData { get; private set; }
        
        // Audit
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        /// <summary>
        /// Construtor privado para o EF Core
        /// </summary>
        private Form() 
        {
            CadastralData = null!;
            EducationData = null!;
            HealthNeeds = string.Empty;
            Occupation = string.Empty;
            ExperienceTime = string.Empty;
            OccupationOther = string.Empty;
            Skills = string.Empty;
            OtherIssues = string.Empty;
            IncomeExpensesData = null!;
            HousingData = null!;
            
            FamilyMembers = new List<FamilyMember>();
            FamilyNeeds = new List<string>();
            EssentialItems = new List<string>();
            SocialIssues = new List<string>();
        }

        /// <summary>
        /// Construtor para criação de novo Formulário
        /// </summary>
        public Form(
            CadastralData cadastralData,
            IEnumerable<FamilyMember> familyMembers,
            EducationData educationData,
            IEnumerable<string> familyNeeds,
            IEnumerable<string> essentialItems,
            string healthNeeds,
            bool hasProfessionalSkills,
            string occupation,
            string experienceTime,
            string occupationOther,
            string skills,
            IEnumerable<string> socialIssues,
            string otherIssues,
            IncomeExpensesData incomeExpensesData,
            HousingData housingData)
        {
            CadastralData = cadastralData ?? throw new ArgumentNullException(nameof(cadastralData));
            FamilyMembers = new List<FamilyMember>(familyMembers ?? []);
            EducationData = educationData ?? throw new ArgumentNullException(nameof(educationData));
            FamilyNeeds = new List<string>(familyNeeds ?? []);
            EssentialItems = new List<string>(essentialItems ?? []);
            HealthNeeds = healthNeeds ?? string.Empty;
            HasProfessionalSkills = hasProfessionalSkills;
            Occupation = occupation ?? string.Empty;
            ExperienceTime = experienceTime ?? string.Empty;
            OccupationOther = occupationOther ?? string.Empty;
            Skills = skills ?? string.Empty;
            SocialIssues = new List<string>(socialIssues ?? []);
            OtherIssues = otherIssues ?? string.Empty;
            IncomeExpensesData = incomeExpensesData ?? throw new ArgumentNullException(nameof(incomeExpensesData));
            HousingData = housingData ?? throw new ArgumentNullException(nameof(housingData));
            
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Atualiza dados do formulário
        /// </summary>
        public void Update(
            CadastralData cadastralData,
            IEnumerable<FamilyMember> familyMembers,
            EducationData educationData,
            IEnumerable<string> familyNeeds,
            IEnumerable<string> essentialItems,
            string healthNeeds,
            bool hasProfessionalSkills,
            string occupation,
            string experienceTime,
            string occupationOther,
            string skills,
            IEnumerable<string> socialIssues,
            string otherIssues,
            IncomeExpensesData incomeExpensesData,
            HousingData housingData)
        {
            CadastralData = cadastralData ?? throw new ArgumentNullException(nameof(cadastralData));
            FamilyMembers = new List<FamilyMember>(familyMembers ?? []);
            EducationData = educationData ?? throw new ArgumentNullException(nameof(educationData));
            FamilyNeeds = new List<string>(familyNeeds ?? []);
            EssentialItems = new List<string>(essentialItems ?? []);
            HealthNeeds = healthNeeds ?? string.Empty;
            HasProfessionalSkills = hasProfessionalSkills;
            Occupation = occupation ?? string.Empty;
            ExperienceTime = experienceTime ?? string.Empty;
            OccupationOther = occupationOther ?? string.Empty;
            Skills = skills ?? string.Empty;
            SocialIssues = new List<string>(socialIssues ?? []);
            OtherIssues = otherIssues ?? string.Empty;
            IncomeExpensesData = incomeExpensesData ?? throw new ArgumentNullException(nameof(incomeExpensesData));
            HousingData = housingData ?? throw new ArgumentNullException(nameof(housingData));
            
            UpdatedAt = DateTime.UtcNow;
        }
    }

    #region Value Objects
    
    /// <summary>
    /// Value Object para Dados Cadastrais
    /// Imutável e valido
    /// </summary>
    public record CadastralData(
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

    /// <summary>
    /// Value Object para Membro da Família
    /// </summary>
    public record FamilyMember(
        string Name,
        int Age,
        string Relationship,
        string Occupation,
        string Schooling);

    /// <summary>
    /// Value Object para Dados Educacionais
    /// </summary>
    public record EducationData(
        string SchoolingLevel,
        bool IsAttendingSchool,
        string Course,
        bool HasProfessionalTraining,
        string TrainingArea);

    /// <summary>
    /// Value Object para Dados de Renda e Despesas
    /// </summary>
    public record IncomeExpensesData(
        decimal MonthlyFamilyIncome,
        decimal TotalExpenses,
        decimal RentValue,
        bool ReceivesGovernmentBenefit,
        string BenefitType,
        decimal BenefitValue);

    /// <summary>
    /// Value Object para Dados Habitacionais
    /// </summary>
    public record HousingData(
        string HousingType,
        bool OwnProperty,
        int RoomsCount,
        int BedroomsCount,
        bool HasBasicSanitation,
        bool HasElectricity,
        bool HasRunningWater);

    #endregion
}