using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;


namespace Service
{
    using EF;
    using EF.Models;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    namespace Service
    {
        public abstract class BaseService<TEntity, TDto> : IBaseService<TEntity, TDto> where TEntity : class where TDto : class
        {
            protected readonly ISenProContext _context;
            protected readonly DbSet<TEntity> _dbSet;

            public BaseService(ISenProContext context)
            {
                _context = context;
                _dbSet = _context.Set<TEntity>();
            }

            public virtual async Task<IEnumerable<TDto>> GetAllAsync()
            {
                var entities = await _dbSet.Where(e => EF.Property<bool>(e, "IsActive")).ToListAsync();
                return entities.Select(MapToDto).ToList();
            }

            public virtual async Task<IEnumerable<TDto>> GetAllWithInActiveAsync()
            {
                var entities = await _dbSet.ToListAsync();
                return entities.Select(MapToDto).ToList();
            }

            public virtual async Task<TDto> GetByIdAsync(int id)
            {
                var entity = await _dbSet.FindAsync(id);
                return entity != null ? MapToDto(entity) : null;
            }

            public virtual async Task<TDto> GetByIdAsync(string id)
            {
                var entity = await _dbSet.FindAsync(id);
                return entity != null ? MapToDto(entity) : null;
            }

            public virtual async Task AddAsync(TDto dto)
            {
                var entity = MapToEntity(dto);
                await _dbSet.AddAsync(entity);
                await _context.SaveChangesAsync();
            }

            public virtual async Task UpdateAsync(TDto dto)
            {
                var entity = MapToEntity(dto);
                _dbSet.Update(entity);
                await _context.SaveChangesAsync();
            }

            public virtual async Task DeleteAsync(string id)
            {
                var entity = await _dbSet.FindAsync(id);
                if (entity != null)
                {
                    _context.Entry(entity).Property("IsActive").CurrentValue = false;
                    await _context.SaveChangesAsync();
                }
            }

            public virtual async Task DeleteAsync(int id)
            {
                var entity = await _dbSet.FindAsync(id);
                if (entity != null)
                {
                    _context.Entry(entity).Property("IsActive").CurrentValue = false;
                    await _context.SaveChangesAsync();
                }
            }

            public virtual async Task<(IEnumerable<TDto> Data, int TotalRecords)> GetPagedAndFilteredAsync(PagingParameters pagingParameters)
            {
                var query = _dbSet.Where(e => EF.Property<bool>(e, "IsActive"));

                if (!string.IsNullOrEmpty(pagingParameters.SearchQuery))
                {
                    query = ApplySearchFilter(query, pagingParameters.SearchQuery);
                }

                var totalRecords = await query.CountAsync();

                var data = await query.Skip((pagingParameters.PageNumber - 1) * pagingParameters.PageSize)
                                      .Take(pagingParameters.PageSize)
                                      .ToListAsync();

                var dtoData = data.Select(MapToDto).ToList();
                return (dtoData, totalRecords);
            }

            protected abstract IQueryable<TEntity> ApplySearchFilter(IQueryable<TEntity> query, string searchQuery);

            // Mapping methods
            protected abstract TDto MapToDto(TEntity entity);
            protected abstract TEntity MapToEntity(TDto dto);
        }
    }

}
