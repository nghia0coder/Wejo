namespace Wejo.Identity.Application.Interfaces;

using Common.Domain.Dtos;
using Request;

public interface IUserChatService
{
    /// <summary>
    /// Gets messages based on specified filters
    /// </summary>
    /// <param name="UserId">User identifier</param>
    /// <param name="userId">User identifier</param>
    /// <param name="request">Request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of messages matching the criteria</returns>
    Task<UserChatMessageDto> SendMessageAsync(Guid UserId, string userId, UserChatSendMessageR request, CancellationToken cancellationToken);

    /// <summary>
    /// Gets conversation messages between two users
    /// </summary>
    /// <param name="userId1">User identifier</param>
    /// <param name="userId2">User identifier</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Conversation infomation</returns>
    Task<Conversation> GetConversationAsync(string userId1, string userId2, CancellationToken cancellationToken);

    ///// <summary>
    ///// Gets messages based on specified filters
    ///// </summary>
    ///// <param name="UserId">User identifier</param>
    ///// <param name="before">Get messages before this date</param>
    ///// <param name="after">Get messages after this date</param>
    ///// <param name="fromUserId">Filter by user ID</param>
    ///// <param name="limit">Maximum number of messages to return</param>
    ///// <param name="cancellationToken">Cancellation token</param>
    ///// <returns>List of messages matching the criteria</returns>
    //Task<List<UserChatMessageDto>> GetMessagesAsync(
    //    Guid UserId,
    //    DateTime? before,
    //    DateTime? after,
    //    string? fromUserId,
    //    int limit,
    //    CancellationToken cancellationToken);

    ///// <summary>
    ///// Gets read status for specified users in a User
    ///// </summary>
    ///// <param name="UserId">User identifier</param>
    ///// <param name="userIds">List of user IDs</param>
    ///// <param name="cancellationToken">Cancellation token</param>
    ///// <returns>Dictionary mapping user IDs to their read status</returns>
    //Task<(Guid? LastReadMessageId, DateTime? LastReadTimestamp)> GetReadStatusAsync(Guid UserId, string userId, CancellationToken cancellationToken);

    ///// <summary>
    ///// Updates the read status for a user in a User
    ///// </summary>
    ///// <param name="UserId">User identifier</param>
    ///// <param name="userId">User identifier</param>
    ///// <param name="lastReadMessageId">Last read message ID</param>
    ///// <param name="lastReadTimestamp">Timestamp of the last read</param>
    ///// <param name="cancellationToken">Cancellation token</param>
    //Task UpdateReadStatusAsync(
    //    Guid UserId,
    //    string userId,
    //    Guid lastReadMessageId,
    //    DateTime lastReadTimestamp,
    //    CancellationToken cancellationToken);
}
