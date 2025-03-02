using System;
using System.Collections.Generic;

namespace Wejo.Common.Domain.Entities.Games;

public partial class GameType
{
    public int Id { get; set; }

    public string NameType { get; set; } = null!;

    public virtual ICollection<Game> Games { get; set; } = new List<Game>();
}
