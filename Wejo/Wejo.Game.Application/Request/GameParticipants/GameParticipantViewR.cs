namespace Wejo.Game.Application.Request;

using Common.Core.Requests;

/// <summary>
/// Request
/// </summary>
public class GameParticipantViewR : PagingR
{
    /// <summary>
    /// GameId
    /// </summary>
    public Guid GameId { get; set; }
}
