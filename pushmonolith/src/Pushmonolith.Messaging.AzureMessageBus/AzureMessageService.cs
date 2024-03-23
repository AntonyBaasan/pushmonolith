using Pushmonolith.Messaging.Abstract.Models;
using Pushmonolith.Messaging.Abstract.Services;

namespace Pushmonolith.Messaging.AzureMessageBus
{
    public class AzureMessageService<TMessageType> : IMessageService<IMessage>
    {
        public Task AddAsync(IMessage item, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(string messageId, string messageReceipt, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IMessage> GetAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<int> LengthAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<string> UpdateAsync(string messageId, string messageReceipt, TimeSpan timeSpan, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
