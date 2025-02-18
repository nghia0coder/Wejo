namespace Wejo.Common.SeedWork.Dtos;

using Interfaces;

/// <summary>
/// Id data transfer object
/// </summary>
public class IdDto : IEntityId<Guid>
{
    #region -- Implements --

    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; set; }

    #endregion
}
