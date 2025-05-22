namespace Wejo.Common.Domain.Entities;

public partial class Venue
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Address { get; set; }

    public virtual ICollection<Game> Games { get; set; } = new List<Game>();
}
