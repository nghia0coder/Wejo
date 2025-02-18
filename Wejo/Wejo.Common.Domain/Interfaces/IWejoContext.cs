using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Wejo.Common.Domain.Interfaces;

using Entities;

/// <summary>
/// Interface McsgContext
/// </summary>
public interface IWejoContext
{
    #region -- Methods --

    /// <summary>
    /// Saves all changes made in this context to the database
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Return the result</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Saves all changes made in this context to the database.
    /// </summary>
    /// <returns>The number of state entries written to the database</returns>
    int SaveChanges();

    /// <summary>
    /// Set
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    /// <returns>Return the result</returns>
    DbSet<T> Set<T>() where T : class;

    #endregion

    #region -- Properties --

    /// <summary>
    /// Database
    /// </summary>
    DatabaseFacade Database { get; }

    #region -- DbSet --

    DbSet<User> Users { get; set; }


    #endregion

    #endregion
}
