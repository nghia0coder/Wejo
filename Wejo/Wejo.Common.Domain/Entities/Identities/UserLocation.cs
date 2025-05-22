namespace Wejo.Common.Domain.Entities;

public partial class UserLocation
{
    public Guid Id { get; set; }

    public string UserId { get; set; } = null!;

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }

    public string? Adddress { get; set; }

    public TimeOnly? UpdatedAt { get; set; }

    public virtual User User { get; set; } = null!;
}
