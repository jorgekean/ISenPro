using EF.Models;
using Service.Dto.Transaction;
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
        Task<(IEnumerable<TDynamic> Data, int TotalRecords)> GetComplexPagedAndFilteredAsync<TDynamic>(PagingParameters pagingParameters) where TDynamic : class;
        Task<object> AddAsync(TDto dto);
        Task UpdateAsync(TDto dto);
        Task DeleteAsync(string id);
        Task DeleteAsync(int id);

        Task<TransactionStatus> AddTransactionStatus(int moduleId, int id,
                UserTransactionPermissions userTransactionPermissions,
                TransactionStatusDto transactionStatusDto);
        Task<string> GetTransactionStatus(int workStepId, TransactionStatusDto transactionStatus);
        Task DisableTransactionStatuses(int moduleId, int id);
    } 
}
