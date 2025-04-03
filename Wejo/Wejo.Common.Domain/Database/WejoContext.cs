using Microsoft.EntityFrameworkCore;

namespace Wejo.Common.Domain.Database;

using Wejo.Common.Core.Enums;
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

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<NotificationSetting> Notificationsettings { get; set; }

    public virtual DbSet<ParticipantHistory> ParticipantHistories { get; set; }

    public virtual DbSet<Sport> Sports { get; set; }

    public virtual DbSet<SportFormat> SportFormats { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserLocation> UserLocations { get; set; }

    public virtual DbSet<UserPlaypal> UserPlaypals { get; set; }

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
                .HasColumnType("timestamp(0) without time zone");
            entity.Property(e => e.EndTime).HasColumnType("timestamp without time zone");
            entity.Property(e => e.GameAccess).HasDefaultValue(true);
            entity.Property(e => e.GameSkill).HasDefaultValue(false);
            entity.Property(e => e.Location).HasColumnType("geometry(Point,4326)");
            entity.Property(e => e.StartTime).HasColumnType("timestamp without time zone");

            // ✅ Corrected Enum Storage: Convert Enum to Integer
            entity.Property(e => e.Status)
                .HasDefaultValue(GameStatus.Waiting) // Set default as Enum, not int
                .HasConversion<int>(); // Convert Enum to Integer

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
            entity.Property(e => e.ModifiedOn).HasColumnType("timestamp(0) without time zone");
            entity.Property(e => e.CreatedOn).HasColumnType("timestamp(0) without time zone");

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

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.ToTable("notifications", "notification");

            entity.HasIndex(e => e.CreatedOn, "IX_Notifications_CreatedOn").IsDescending();

            entity.HasIndex(e => e.UserId, "IX_Notifications_UserId");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasComment("Unique identifier for the notification");
            entity.Property(e => e.CreatedOn).HasComment("Timestamp when the notification was created");
            entity.Property(e => e.IsRead)
                .HasDefaultValue(false)
                .HasComment("Indicates if the notification has been read");
            entity.Property(e => e.Message).HasComment("Detailed message content");
            entity.Property(e => e.RelatedEntityId).HasComment("Optional ID of related entity (e.g., GameId)");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasComment("Short title of the notification");
            entity.Property(e => e.Type).HasComment("Notification type (enum: 1=GameStarted, 2=GameEnded, 3=PlaypalAdded, etc.)");
            entity.Property(e => e.UserId)
                .HasMaxLength(255)
                .HasComment("Recipient user ID");
        });


        modelBuilder.Entity<NotificationSetting>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.NotificationType }).HasName("PK_NotificationSettings");

            entity.ToTable("notificationsettings", "notification", tb => tb.HasComment("Stores user preferences for notification types"));

            entity.HasIndex(e => e.UserId, "IX_NotificationSettings_UserId");

            entity.Property(e => e.UserId)
                .HasMaxLength(255)
                .HasComment("User ID");
            entity.Property(e => e.NotificationType).HasComment("Type of notification (enum)");
            entity.Property(e => e.IsEnabled)
                .HasDefaultValue(true)
                .HasComment("Whether this notification type is enabled for the user");
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

        modelBuilder.Entity<UserPlaypal>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("UserPlaypal_pkey");

            entity.ToTable("UserPlaypal", "identity");

            entity.HasIndex(e => new { e.UserId1, e.UserId2 }, "uq_playpals_users").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedOn).HasColumnType("timestamp without time zone");
            entity.Property(e => e.UserId1).HasMaxLength(28);
            entity.Property(e => e.UserId2).HasMaxLength(28);

            entity.HasOne(d => d.Game).WithMany(p => p.UserPlaypals)
                .HasForeignKey(d => d.GameId)
                .HasConstraintName("FK_Playpals_GameId");

            entity.HasOne(d => d.UserId1Navigation).WithMany(p => p.UserPlaypalUserId1Navigations)
                .HasForeignKey(d => d.UserId1)
                .HasConstraintName("FK_Playpals_UserId1");

            entity.HasOne(d => d.UserId2Navigation).WithMany(p => p.UserPlaypalUserId2Navigations)
                .HasForeignKey(d => d.UserId2)
                .HasConstraintName("FK_Playpals_UserId2");
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
