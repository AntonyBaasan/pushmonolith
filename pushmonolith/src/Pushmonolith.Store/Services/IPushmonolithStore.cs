using Pushmonolith.Store.Models;

namespace Pushmonolith.Store.Services
{
    public interface IPushmonolithStore
    {
        Task<string> Create<T>(T item) where T : AbstractItem;
        Task<T> GetById<T>(string id) where T : AbstractItem;
        Task<bool> Delete<T>(string id) where T : AbstractItem;
        Task<IList<T>> GetAll<T>() where T : AbstractItem;
    }
}
