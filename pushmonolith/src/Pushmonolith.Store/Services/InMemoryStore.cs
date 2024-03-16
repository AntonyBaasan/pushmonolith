namespace Pushmonolith.Store.Services
{
    public class InMemoryStore : IPushmonolithStore 
    {
        public Dictionary<string, object> _store = new Dictionary<string, object>();
        public string Create<T>(T item)
        {
            string uid = Guid.NewGuid().ToString();
            _store.Add(uid, item);
            return uid;
        }

        public T GetById<T>(string id)
        {
            return (T)_store[id];
        }

        Task<string> IPushmonolithStore.Create<T>(T item)
        {
            string uid = Guid.NewGuid().ToString();
            item.Id = uid;
            _store.Add(uid, item);
            return Task.FromResult(uid);
        }

        Task<bool> IPushmonolithStore.Delete<T>(string id)
        {
            _store.Remove(id);
            return Task.FromResult(true);
        }

        Task<IList<T>> IPushmonolithStore.GetAll<T>()
        {
            IList<T> items = _store.Values.Cast<T>().ToList();
            return Task.FromResult(items);
        }

        Task<T> IPushmonolithStore.GetById<T>(string id)
        {
            return Task.FromResult((T)_store[id]);
        }
    }
}
