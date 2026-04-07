using Microsoft.EntityFrameworkCore;
using CadastroFamilias.Core.Entities;
using System.Text.Json;

namespace CadastroFamilias.Infrastructure.Persistence
{
    /// <summary>
    /// Contexto do Banco de Dados SQLite
    /// Implementação concreta na camada Infrastructure
    /// </summary>
    public class AppDbContext : DbContext
    {
        public DbSet<Form> Forms { get; set; }

        /// <summary>
        /// Configurações padrão para serialização JSON
        /// </summary>
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuração da entidade Form
            modelBuilder.Entity<Form>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.CreatedAt)
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");
                
                entity.Property(e => e.UpdatedAt)
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");

                // Conversões para Value Objects (serialização JSON)
                entity.OwnsOne(e => e.CadastralData, owned =>
                {
                    owned.ToJson("cadastral_data");
                });
                
                entity.OwnsMany(e => e.FamilyMembers, owned =>
                {
                    owned.ToJson("family_members");
                });
                
                entity.OwnsOne(e => e.EducationData, owned =>
                {
                    owned.ToJson("education_data");
                });
                
                entity.Property(e => e.FamilyNeeds)
                      .HasConversion(
                          v => JsonSerializer.Serialize(v, JsonOptions),
                          v => JsonSerializer.Deserialize<List<string>>(v, JsonOptions) ?? new List<string>())
                      .HasColumnName("family_needs");
                
                entity.Property(e => e.EssentialItems)
                      .HasConversion(
                          v => JsonSerializer.Serialize(v, JsonOptions),
                          v => JsonSerializer.Deserialize<List<string>>(v, JsonOptions) ?? new List<string>())
                      .HasColumnName("essential_items");
                
                entity.Property(e => e.HealthNeeds)
                      .HasColumnName("health_needs");
                
                entity.Property(e => e.HasProfessionalSkills)
                      .HasColumnName("has_professional_skills");
                
                entity.Property(e => e.ExperienceTime)
                      .HasColumnName("experience_time");
                
                entity.Property(e => e.OccupationOther)
                      .HasColumnName("occupation_other");
                
                entity.Property(e => e.SocialIssues)
                      .HasConversion(
                          v => JsonSerializer.Serialize(v, JsonOptions),
                          v => JsonSerializer.Deserialize<List<string>>(v, JsonOptions) ?? new List<string>())
                      .HasColumnName("social_issues");
                
                entity.Property(e => e.OtherIssues)
                      .HasColumnName("other_issues");
                
                entity.OwnsOne(e => e.IncomeExpensesData, owned =>
                {
                    owned.ToJson("income_expenses_data");
                });
                
                entity.OwnsOne(e => e.HousingData, owned =>
                {
                    owned.ToJson("housing_data");
                });
            });
        }

        /// <summary>
        /// Atualiza campo UpdatedAt automaticamente ao salvar
        /// </summary>
        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        /// <summary>
        /// Atualiza campo UpdatedAt automaticamente ao salvar assincrono
        /// </summary>
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        {
            var entries = ChangeTracker.Entries<Form>()
                .Where(e => e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                entry.Entity.GetType()
                    .GetProperty("UpdatedAt")?
                    .SetValue(entry.Entity, DateTime.UtcNow);
            }
        }
    }
}