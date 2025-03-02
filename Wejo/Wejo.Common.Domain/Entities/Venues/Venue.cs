using System;
using System.Collections.Generic;
using Wejo.Common.Domain.Entities.Games;

namespace Wejo.Common.Domain.Entities.Venues;

public partial class Venue
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Address { get; set; }

    public virtual ICollection<Game> Games { get; set; } = new List<Game>();
}
