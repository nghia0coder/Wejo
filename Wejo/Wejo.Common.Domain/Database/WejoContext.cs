using Microsoft.EntityFrameworkCore;

namespace Wejo.Common.Domain.Database;

using Wejo.Common.Domain.Entities;
using Wejo.Common.Domain.Interfaces;

public partial class WejoContext : DbContext, IWejoContext
{
    public WejoContext()
    {
    }

    public WejoContext(DbContextOptions<WejoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Game> Games { get; set; }

    public virtual DbSet<GameParticipant> GameParticipants { get; set; }

    public virtual DbSet<GameType> GameTypes { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<ParticipantHistory> ParticipantHistories { get; set; }

    public virtual DbSet<Sport> Sports { get; set; }

    public virtual DbSet<SportFormat> SportFormats { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserLocation> UserLocations { get; set; }

    public virtual DbSet<Venue> Venues { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("uuid-ossp");

        modelBuilder.Entity<Game>(entity =>
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

        modelBuilder.Entity<GameParticipant>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("GameParticipants_pkey");

            entity.ToTable("GameParticipants", "game");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.JoinedAt).HasColumnType("timestamp(0) without time zone");
            entity.Property(e => e.LeftAt).HasColumnType("timestamp(0) without time zone");

            entity.HasOne(d => d.Game).WithMany(p => p.GameParticipants)
                .HasForeignKey(d => d.GameId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pk_gameid");

            entity.HasOne(d => d.User).WithMany(p => p.GameParticipants)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pk_userid");
        });

        modelBuilder.Entity<GameType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("GameType_pkey");

            entity.ToTable("GameType", "game");

            entity.Property(e => e.NameType).HasMaxLength(255);
        });

        modelBuilder.Entity<Message>(entity =>
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

        modelBuilder.Entity<ParticipantHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ParticipantHistory_pkey");

            entity.ToTable("ParticipantHistory", "game");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Details).HasMaxLength(255);

            entity.HasOne(d => d.Participant).WithMany(p => p.ParticipantHistories)
                .HasForeignKey(d => d.ParticipantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pk_participantid");
        });

        modelBuilder.Entity<Sport>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Sport_pkey");

            entity.ToTable("Sport", "sport");

            entity.HasIndex(e => e.Name, "Sport_Name_key").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<SportFormat>(entity =>
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

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Users_pkey");

            entity.ToTable("User", "identity");
        });

        modelBuilder.Entity<UserLocation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("UserLocation_pkey");

            entity.ToTable("UserLocation", "identity");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Latitude).HasPrecision(10, 6);
            entity.Property(e => e.Longitude).HasPrecision(10, 6);

            entity.HasOne(d => d.User).WithMany(p => p.UserLocations)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_userId");
        });

        modelBuilder.Entity<Venue>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Venue_pkey");

            entity.ToTable("Venue", "venue");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
