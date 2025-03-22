using Wejo.Common.Core.Requests;

namespace Wejo.Game.Application.Request.Games
{
    public class GameListInfoR : PagingR
    {
        public Guid GameId { get; set; }
        public int SportId { get; set; }
        public DateOnly? Date { get; set; }
        public TimeOnly? TimeStart { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public int? SkillStart { get; set; }
        public int? SkillEnd { get; set; }

    }
}
