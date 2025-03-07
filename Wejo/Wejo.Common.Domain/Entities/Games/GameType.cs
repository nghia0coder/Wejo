namespace Wejo.Common.Domain.Entities;

public partial class GameType
{
    public int Id { get; set; }

    public string NameType { get; set; } = null!;

    public virtual ICollection<Game> Games { get; set; } = new List<Game>();
}
