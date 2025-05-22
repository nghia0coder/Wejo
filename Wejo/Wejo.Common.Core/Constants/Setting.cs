namespace Wejo.Common.Core.Constants;

using Enums;

/// <summary>
/// Setting
/// </summary>
public class Setting : SeedWork.Constants.Setting
{
    /// <summary>
    /// MicroServices
    /// </summary>
    public static Dictionary<string, string> MicroServices
    {
        get
        {
            return new Dictionary<string, string>
            {
                { "Ide", MicroService.Identity.ToString() },
                { "Gam", MicroService.Game.ToString() }
            };
        }
    }
}
