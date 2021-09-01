using System.Collections.Generic;
using TrailerScope.Contracts.Services;

namespace TrailerScope.Services.Caching
{
    public class MemoryCache <T>: ICache<T>
    {
        private Dictionary<string, T> cache = new ();

        public bool Contains(string key) => cache.ContainsKey(key);

        public T GetItem(string key) => cache.ContainsKey(key) ? cache[key] : default(T);

        public void AddItem(string key, T item) => cache.Add(key,item);
    }
}