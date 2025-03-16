namespace Wejo.Common.Domain.Entities;

using SeedWork;

public partial class UserPlaypal : EntityId
{
    public string? UserId1 { get; set; }

    public string? UserId2 { get; set; }

    public Guid? GameId { get; set; }

    public DateTime? CreatedOn { get; set; }

    public virtual Game? Game { get; set; }

    public virtual User? UserId1Navigation { get; set; }

    public virtual User? UserId2Navigation { get; set; }
}
