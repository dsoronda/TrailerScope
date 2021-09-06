namespace TrailerScope.Contracts.Services
{
    public interface ICache<T>
    {
        bool Contains(string key);

        T GetItem(string key);

        void AddItem(string key,T item);
    }
}
