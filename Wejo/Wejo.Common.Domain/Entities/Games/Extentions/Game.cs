namespace Wejo.Common.Domain.Entities.Games.Extentions
{
    public static class GameExtensions
    {
        // ✅ Create a new Game instance
        public static Games.Game CreateGame(
            int sportId,
            string createdBy,
            int? sportFormatId,
            Guid? venueId,
            int? gameTypeId,
            string area,
            DateOnly date,
            TimeOnly startTime,
            TimeOnly endTime,
            bool gameAccess,
            bool bringEquipment,
            bool costShared,
            bool gameSkill,
            int? skillStart,
            int? skillEnd,
            int? totalPlayer,
            int? status,
            string? description)
        {
            return new Games.Game
            {
                Id = Guid.NewGuid(),
                SportId = sportId,
                CreatedBy = createdBy,
                SportFormatId = sportFormatId,
                VenueId = venueId,
                GameTypeId = gameTypeId,
                Area = area,
                Date = date,
                StartTime = startTime,
                EndTime = endTime,
                GameAccess = gameAccess,
                BringEquipment = bringEquipment,
                CostShared = costShared,
                GameSkill = gameSkill,
                SkillStart = skillStart,
                SkillEnd = skillEnd,
                TotalPlayer = totalPlayer,
                Status = status,
                Description = description,
                CreatedAt = DateTime.UtcNow.ToLocalTime() // Auto-set timestamp
            };
        }


    }
}
