namespace Wejo.Game.Application.Commands;

using Common.Domain.Interfaces;

public abstract class BaseH(IWejoContext context)
{
    protected readonly IWejoContext _context = context;
}
