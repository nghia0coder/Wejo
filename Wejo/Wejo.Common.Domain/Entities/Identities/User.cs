namespace Wejo.Common.Domain.Entities;

using Core.Enums;

public partial class User
{
    public string? Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public Gender? Gender { get; set; }

    public string? RefreshToken { get; set; }

    public DateTime? RefreshTokenExpiryTime { get; set; }

    public string? Bio { get; set; }

    public int? Level { get; set; }

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

    public bool EmailConfirmed { get; set; }
}