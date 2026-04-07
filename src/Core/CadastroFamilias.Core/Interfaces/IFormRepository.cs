using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CadastroFamilias.Core.Entities;

namespace CadastroFamilias.Core.Interfaces
{
    /// <summary>
    /// Porta (Interface) para Repositório de Formulários
    /// Definida na camada Core, implementada na camada Infrastructure
    /// Segue Arquitetura Hexagonal e Princípio da Inversão de Dependência
    /// </summary>
    public interface IFormRepository
    {
        /// <summary>
        /// Busca formulário por Id
        /// </summary>
        Task<Form?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Retorna todos os formulários cadastrados
        /// </summary>
        Task<IReadOnlyCollection<Form>> GetAllAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Busca formulários com filtros
        /// </summary>
        Task<IReadOnlyCollection<Form>> SearchAsync(FormSearchFilters filters, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Cria novo formulário
        /// </summary>
        Task<Form> CreateAsync(Form form, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Atualiza formulário existente
        /// </summary>
        Task UpdateAsync(Form form, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Exclui formulário
        /// </summary>
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Verifica se existe formulário com Id informado
        /// </summary>
        Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Filtros para busca de formulários
    /// </summary>
    public record FormSearchFilters(
        string? ApplicantName = null,
        string? City = null,
        string? State = null,
        bool? HasFamilyNeeds = null,
        int? MinAge = null,
        int? MaxAge = null);
}