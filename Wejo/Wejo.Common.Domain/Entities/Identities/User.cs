namespace Wejo.Common.Domain.Entities;

using Wejo.Common.Core.Enums;

public partial class User
{
    public string Id { get; set; } = null!;

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public Gender? Gender { get; set; }

    public string? Bio { get; set; }

    public PlayerLevel? Level { get; set; }

    public string? Avatar { get; set; }

    public DateTime? ActivedDate { get; set; }

    public DateTime? LastLoginDate { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public bool? IsDelete { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public bool? PhoneNumberConfirmed { get; set; }

    public bool? EmailConfirmed { get; set; }

    public virtual ICollection<GameParticipant> GameParticipants { get; set; } = new List<GameParticipant>();

    public virtual ICollection<UserLocation> UserLocations { get; set; } = new List<UserLocation>();

    public virtual ICollection<UserPlaypal> UserPlaypalUserId1Navigations { get; set; } = new List<UserPlaypal>();

    public virtual ICollection<UserPlaypal> UserPlaypalUserId2Navigations { get; set; } = new List<UserPlaypal>();
}
