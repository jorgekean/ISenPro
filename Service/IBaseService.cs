using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IBaseService<TEntity, TDto> where TEntity : class where TDto : class
    {
        Task<IEnumerable<TDto>> GetAllAsync();
        Task<IEnumerable<TDto>> GetAllWithInActiveAsync();
        Task<TDto> GetByIdAsync(int id);
        Task<TDto> GetByIdAsync(string id);
        Task<(IEnumerable<TDto> Data, int TotalRecords)> GetPagedAndFilteredAsync(PagingParameters pagingParameters);
        Task AddAsync(TDto dto);
        Task UpdateAsync(TDto dto);
        Task DeleteAsync(string id);
    }    
}
