namespace Wejo.Game.Application.Request;

using Common.Core.Requests;

/// <summary>
/// Request
/// </summary>
public class GameParticipantUpdateR : IdBaseR
{
    /// <summary>
    /// GameId
    /// </summary>
    public Guid GameId { get; set; }

    /// <summary>
    /// PlayerStatus
    /// </summary>
    public string? PlayerStatus { get; set; }
}
