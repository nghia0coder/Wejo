using Microsoft.EntityFrameworkCore;

namespace Wejo.Common.Domain.Database;

using Entities;
using Interfaces;

public partial class WejoContext : DbContext, IWejoContext
{
    public WejoContext()
    {
    }

    public WejoContext(DbContextOptions<WejoContext> options) : base(options) { }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(WejoContext).Assembly);


        builder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Users_pkey");

            entity.ToTable("Users", "identity");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Gender).HasMaxLength(10);
        });
    }
}
