using Wejo.Common.Core.Requests;

namespace Wejo.Game.Application.Request.Games
{
    public class GameHistoryR : PagingR
    {
        public string UserId { get; set; } = null!;
        public string? TimeType { get; set; }
    }
}
