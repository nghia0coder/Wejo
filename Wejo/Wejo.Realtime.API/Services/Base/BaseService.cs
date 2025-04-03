namespace Wejo.Realtime.API.Services;

using Common.Domain.Interfaces;

public interface IBaseService
{
    IWejoContext Context { get; }
}

public abstract class BaseService : IBaseService
{
    public IWejoContext Context => _context;
    protected readonly IWejoContext _context;

    public BaseService(IWejoContext context)
    {
        _context = context;
    }
}

public abstract class BaseSettingS : BaseService
{
    protected readonly ISetting _setting;

    public BaseSettingS(IWejoContext context, ISetting setting) : base(context)
    {
        _setting = setting;
    }
}