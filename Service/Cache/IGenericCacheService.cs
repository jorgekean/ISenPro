using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Cache
{
    public interface IGenericCacheService
    {
        T Get<T>(string key);
        void Set<T>(string key, T value, TimeSpan expiration);
        Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> createItem, TimeSpan expiration);
        void Remove(string key);
        void Clear();
    }
}
