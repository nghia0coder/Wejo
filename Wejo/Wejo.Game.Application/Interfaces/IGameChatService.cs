namespace Wejo.Game.Application.Interfaces;

using Common.Domain.Dtos;
using Request;

public interface IGameChatService
{
    /// <summary>
    /// Gets messages based on specified filters
    /// </summary>
    /// <param name="gameId">Game identifier</param>
    /// <param name="userId">User identifier</param>
    /// <param name="request">Request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of messages matching the criteria</returns>
    Task<GameChatMessageDto> SendMessageAsync(Guid gameId, string userId, GameChatSendMessageR request, CancellationToken cancellationToken);

    /// <summary>
    /// Gets messages based on specified filters
    /// </summary>
    /// <param name="gameId">Game identifier</param>
    /// <param name="before">Get messages before this date</param>
    /// <param name="after">Get messages after this date</param>
    /// <param name="fromUserId">Filter by user ID</param>
    /// <param name="limit">Maximum number of messages to return</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of messages matching the criteria</returns>
    Task<List<GameChatMessageDto>> GetMessagesAsync(
        Guid gameId,
        DateTime? before,
        DateTime? after,
        string? fromUserId,
        int limit,
        CancellationToken cancellationToken);

    /// <summary>
    /// Gets read status for specified users in a game
    /// </summary>
    /// <param name="gameId">Game identifier</param>
    /// <param name="userIds">List of user IDs</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Dictionary mapping user IDs to their read status</returns>
    Task<Dictionary<string, (Guid? LastReadMessageId, DateTime? LastReadTimestamp)>> GetReadStatusAsync(
        Guid gameId,
        List<string> userIds,
        CancellationToken cancellationToken);

    /// <summary>
    /// Updates the read status for a user in a game
    /// </summary>
    /// <param name="gameId">Game identifier</param>
    /// <param name="userId">User identifier</param>
    /// <param name="lastReadMessageId">Last read message ID</param>
    /// <param name="lastReadTimestamp">Timestamp of the last read</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task UpdateReadStatusAsync(
        Guid gameId,
        string userId,
        Guid lastReadMessageId,
        DateTime lastReadTimestamp,
        CancellationToken cancellationToken);
}
