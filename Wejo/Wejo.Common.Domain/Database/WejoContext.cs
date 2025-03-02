using Microsoft.EntityFrameworkCore;

namespace Wejo.Common.Domain.Database;

using Entities;
using Interfaces;
using Wejo.Common.Domain.Entities.Games;
using Wejo.Common.Domain.Entities.Messages;
using Wejo.Common.Domain.Entities.Sports;
using Wejo.Common.Domain.Entities.Venues;

public partial class WejoContext : DbContext, IWejoContext
{
    public WejoContext()
    {
    }

    public WejoContext(DbContextOptions<WejoContext> options) : base(options) { }

    public virtual DbSet<Game> Games { get; set; }

    public virtual DbSet<GamePlayerDetail> GamePlayerDetails { get; set; }

    public virtual DbSet<GameType> GameTypes { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<Sport> Sports { get; set; }

    public virtual DbSet<SportFormat> SportFormats { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Venue> Venues { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        //builder.ApplyConfigurationsFromAssembly(typeof(WejoContext).Assembly);
        builder.HasPostgresExtension("uuid-ossp");

        builder.Entity<Game>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Game_pkey");

            entity.ToTable("Game", "game");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.Area).HasMaxLength(255);
            entity.Property(e => e.BringEquipment).HasDefaultValue(false);
            entity.Property(e => e.CostShared).HasDefaultValue(false);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.GameAccess).HasDefaultValue(true);
            entity.Property(e => e.GameSkill).HasDefaultValue(false);
            entity.Property(e => e.Status).HasDefaultValue(0);

            entity.HasOne(d => d.GameType).WithMany(p => p.Games)
                .HasForeignKey(d => d.GameTypeId)
                .HasConstraintName("Game_GameTypeId_fkey");

            entity.HasOne(d => d.SportFormat).WithMany(p => p.Games)
                .HasForeignKey(d => d.SportFormatId)
                .HasConstraintName("Game_SportFormatId_fkey");

            entity.HasOne(d => d.Sport).WithMany(p => p.Games)
                .HasForeignKey(d => d.SportId)
                .HasConstraintName("Game_SportId_fkey");

            entity.HasOne(d => d.Venue).WithMany(p => p.Games)
                .HasForeignKey(d => d.VenueId)
                .HasConstraintName("Game_VenueId_fkey");
        });

        builder.Entity<GamePlayerDetail>(entity =>
        {
            entity.HasKey(e => new { e.GameId, e.UserId }).HasName("GamePlayerDetail_pkey");

            entity.ToTable("GamePlayerDetail", "game");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.Role).HasDefaultValue(0);
            entity.Property(e => e.Status).HasDefaultValue(0);

            entity.HasOne(d => d.Game).WithMany(p => p.GamePlayerDetails)
                .HasForeignKey(d => d.GameId)
                .HasConstraintName("GamePlayerDetail_GameId_fkey");
        });

        builder.Entity<GameType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("GameType_pkey");

            entity.ToTable("GameType", "game");

            entity.Property(e => e.NameType).HasMaxLength(255);
        });

        builder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Message_pkey");

            entity.ToTable("Message", "message");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.IsComment).HasDefaultValue(false);

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("Message_ParentId_fkey");
        });

        builder.Entity<Sport>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Sport_pkey");

            entity.ToTable("Sport", "sport");

            entity.HasIndex(e => e.Name, "Sport_Name_key").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(255);
        });

        builder.Entity<SportFormat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("GameFormat_pkey");

            entity.ToTable("SportFormat", "sport");

            entity.HasIndex(e => new { e.SportId, e.FormatName }, "GameFormat_SportId_FormatName_key").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("nextval('sport.\"GameFormat_Id_seq\"'::regclass)");
            entity.Property(e => e.FormatName).HasMaxLength(100);

            entity.HasOne(d => d.Sport).WithMany(p => p.SportFormats)
                .HasForeignKey(d => d.SportId)
                .HasConstraintName("GameFormat_SportId_fkey");
        });

        builder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Users_pkey");

            entity.ToTable("Users", "identity");

            entity.Property(e => e.Id).HasColumnType("text").ValueGeneratedNever();

        });

        builder.Entity<Venue>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Venue_pkey");

            entity.ToTable("Venue", "venue");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.Name).HasMaxLength(255);
        });

    }
}
