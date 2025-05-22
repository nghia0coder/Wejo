namespace Wejo.Common.Domain.Entities;

public partial class SportFormat
{
    public int Id { get; set; }

    public int SportId { get; set; }

    public string FormatName { get; set; } = null!;

    public virtual ICollection<Game> Games { get; set; } = new List<Game>();

    public virtual Sport Sport { get; set; } = null!;
}
