namespace Wejo.Common.Domain.Dtos;

using Core.Enums;

public class UserDto
{
    public string Id { get; set; } = null!;
    public string? Bio { get; set; }
    public string? Avatar { get; set; }
    public Gender Gender { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public PlayerLevel Level { get; set; }
    public string FullName { get; set; } = null!;
    public int GamesParticipatedCount { get; set; }
    public int TotalPlaypal { get; set; }
}

public class UserViewDto : UserDto
{
    public string? GenderName { get; set; }

    public string? LevelName { get; set; }
}
