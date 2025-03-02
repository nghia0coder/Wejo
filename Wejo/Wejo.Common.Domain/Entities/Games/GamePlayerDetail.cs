using System;
using System.Collections.Generic;

namespace Wejo.Common.Domain.Entities.Games;

public partial class GamePlayerDetail
{
    public Guid GameId { get; set; }

    public string UserId { get; set; } = null!;

    public int Role { get; set; }

    public int Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Game Game { get; set; } = null!;
}
