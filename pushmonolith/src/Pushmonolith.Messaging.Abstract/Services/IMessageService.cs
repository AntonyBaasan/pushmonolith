using Pushmonolith.Messaging.Abstract.Models;

namespace Pushmonolith.Messaging.Abstract.Services
{
    public interface IMessageService<TMessageType> where TMessageType: IMessage
    {
        /// <summary>
        /// Add message to a queue
        /// </summary>
        /// <param name="item">message</param>
        /// <returns></returns>
        public Task AddAsync(TMessageType item, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieve message from a queue. This call will block until a message is available.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<TMessageType> GetAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// delete message from queue
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="messageReceipt"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task DeleteAsync(string messageId, string messageReceipt, CancellationToken cancellationToken = default);

        /// <summary>
        /// update message in the queue by message id
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="messageReceipt"></param>
        /// <param name="timeSpan"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>new message receipt id</returns>
        public Task<string> UpdateAsync(string messageId, string messageReceipt, TimeSpan timeSpan, CancellationToken cancellationToken = default);

        /// <summary>
        /// get length of the queue
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<int> LengthAsync(CancellationToken cancellationToken = default);
    }
}
