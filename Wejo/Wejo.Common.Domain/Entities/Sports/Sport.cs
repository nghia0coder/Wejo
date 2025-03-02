using System;
using System.Collections.Generic;
using Wejo.Common.Domain.Entities.Games;

namespace Wejo.Common.Domain.Entities.Sports;

public partial class Sport
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Game> Games { get; set; } = new List<Game>();

    public virtual ICollection<SportFormat> SportFormats { get; set; } = new List<SportFormat>();
}
