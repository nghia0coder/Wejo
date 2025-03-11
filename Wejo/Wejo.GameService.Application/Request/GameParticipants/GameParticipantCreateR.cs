namespace Wejo.Game.Application.Request;

using Common.Core.Requests;

/// <summary>
/// Request
/// </summary>
public class GameParticipantCreateR : IdBaseR
{
    /// <summary>
    /// GameId
    /// </summary>
    public Guid GameId { get; set; }
}
