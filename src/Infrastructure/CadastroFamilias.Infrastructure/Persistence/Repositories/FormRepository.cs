using Microsoft.EntityFrameworkCore;
using CadastroFamilias.Core.Entities;
using CadastroFamilias.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CadastroFamilias.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Implementação concreta do Repositório de Formulários para SQLite
    /// Implementa a Porta (Interface) definida na camada Core
    /// </summary>
    public class FormRepository : IFormRepository
    {
        private readonly AppDbContext _context;

        public FormRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Form?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Forms
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == id, cancellationToken);
        }

        public async Task<IReadOnlyCollection<Form>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Forms
                .AsNoTracking()
                .OrderByDescending(f => f.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyCollection<Form>> SearchAsync(FormSearchFilters filters, CancellationToken cancellationToken = default)
        {
            var query = _context.Forms.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(filters.ApplicantName))
            {
                query = query.Where(f => EF.Functions.Like(f.CadastralData.ApplicantName, $"%{filters.ApplicantName}%"));
            }

            if (!string.IsNullOrWhiteSpace(filters.City))
            {
                query = query.Where(f => EF.Functions.Like(f.CadastralData.City, $"%{filters.City}%"));
            }

            if (!string.IsNullOrWhiteSpace(filters.State))
            {
                query = query.Where(f => f.CadastralData.State == filters.State);
            }

            if (filters.HasFamilyNeeds == true)
            {
                query = query.Where(f => f.FamilyNeeds.Any());
            }

            if (filters.MinAge.HasValue)
            {
                query = query.Where(f => f.CadastralData.BirthDate <= DateTime.Now.AddYears(-filters.MinAge.Value));
            }

            if (filters.MaxAge.HasValue)
            {
                query = query.Where(f => f.CadastralData.BirthDate >= DateTime.Now.AddYears(-filters.MaxAge.Value));
            }

            return await query
                .OrderByDescending(f => f.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<Form> CreateAsync(Form form, CancellationToken cancellationToken = default)
        {
            await _context.Forms.AddAsync(form, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return form;
        }

        public async Task UpdateAsync(Form form, CancellationToken cancellationToken = default)
        {
            _context.Entry(form).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var form = await _context.Forms.FindAsync([id], cancellationToken);
            if (form != null)
            {
                _context.Forms.Remove(form);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Forms.AnyAsync(f => f.Id == id, cancellationToken);
        }
    }
}