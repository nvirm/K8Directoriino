using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using static K8Director.Program;

namespace K8Director.Models
{
    public partial class ksBotSQLContext : DbContext
    {
        public virtual DbSet<Match> Match { get; set; }
        public virtual DbSet<MatchTeams> MatchTeams { get; set; }
        public virtual DbSet<MatchUsers> MatchUsers { get; set; }
        public virtual DbSet<Param> Param { get; set; }
        public virtual DbSet<Scoreboard> Scoreboard { get; set; }
        public virtual DbSet<Team> Team { get; set; }
        public virtual DbSet<TeamCaptainUser> TeamCaptainUser { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ProgHelpers.connstring);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Match>(entity =>
            {
                entity.HasIndex(e => e.Matchcode)
                    .HasName("Matchcode")
                    .IsUnique();

                entity.HasIndex(e => e.WinnerTeamId)
                    .HasName("WinnerTeam_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Added)
                    .HasColumnName("added")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.Closed)
                    .HasColumnName("closed")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.ManageDraftUrl)
                    .HasColumnName("ManageDraftURL")
                    .HasMaxLength(255);

                entity.Property(e => e.Matchcode)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Modified)
                    .HasColumnName("modified")
                    .HasColumnType("datetime");

                entity.Property(e => e.ObsDraftUrl)
                    .HasColumnName("ObsDraftURL")
                    .HasMaxLength(255);

                entity.Property(e => e.Projectedstarttime)
                    .HasColumnName("projectedstarttime")
                    .HasColumnType("datetime");

                entity.Property(e => e.Started)
                    .HasColumnName("started")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Team1Division).HasColumnName("Team1_division");

                entity.Property(e => e.Team1Id).HasColumnName("Team1_id");

                entity.Property(e => e.Team2Division).HasColumnName("Team2_division");

                entity.Property(e => e.Team2Id).HasColumnName("Team2_id");

                entity.Property(e => e.WinnerTeamId).HasColumnName("WinnerTeam_id");
            });

            modelBuilder.Entity<MatchTeams>(entity =>
            {
                entity.HasKey(e => new { e.TeamId, e.MatchId })
                    .HasName("aaaaaMatchTeams_PK");

                entity.HasIndex(e => e.MatchId)
                    .HasName("MatchMatchTeams");

                entity.HasIndex(e => e.TeamId)
                    .HasName("TeamMatchTeams");

                entity.HasIndex(e => e.TeamId1CaptainUserId)
                    .HasName("TeamId1CaptainUserId");

                entity.HasIndex(e => e.TeamId2CaptainUserId)
                    .HasName("TeamId2CaptainUserId");

                entity.Property(e => e.TeamId)
                    .HasColumnName("Team_id")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.MatchId)
                    .HasColumnName("Match_id")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.TeamId1CaptainUserId).HasDefaultValueSql("0");

                entity.Property(e => e.TeamId1DraftUrl)
                    .HasColumnName("TeamId1DraftURL")
                    .HasMaxLength(255);

                entity.Property(e => e.TeamId1Points).HasDefaultValueSql("0");

                entity.Property(e => e.TeamId2CaptainUserId).HasDefaultValueSql("0");

                entity.Property(e => e.TeamId2DraftUrl)
                    .HasColumnName("TeamId2DraftURL")
                    .HasMaxLength(255);

                entity.Property(e => e.TeamId2Points).HasDefaultValueSql("0");

                entity.HasOne(d => d.Match)
                    .WithMany(p => p.MatchTeams)
                    .HasForeignKey(d => d.MatchId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("MatchTeams_FK00");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.MatchTeams)
                    .HasForeignKey(d => d.TeamId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("MatchTeams_FK01");
            });

            modelBuilder.Entity<MatchUsers>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.MatchId })
                    .HasName("aaaaaMatchUsers_PK");

                entity.HasIndex(e => e.MatchId)
                    .HasName("MatchMatchUsers");

                entity.HasIndex(e => e.UserId)
                    .HasName("UserMatchUsers");

                entity.Property(e => e.UserId).HasColumnName("User_id");

                entity.Property(e => e.MatchId).HasColumnName("Match_id");

                entity.HasOne(d => d.Match)
                    .WithMany(p => p.MatchUsers)
                    .HasForeignKey(d => d.MatchId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("MatchUsers_FK00");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.MatchUsers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("MatchUsers_FK01");
            });

            modelBuilder.Entity<Param>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ParamInfo).HasMaxLength(255);

                entity.Property(e => e.ParamName).HasMaxLength(255);
            });

            modelBuilder.Entity<Scoreboard>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CityName).HasMaxLength(255);

                entity.Property(e => e.TeamId).HasColumnName("Team_id");
            });

            modelBuilder.Entity<Team>(entity =>
            {
                entity.HasIndex(e => e.CaptainUserId)
                    .HasName("CaptainUser_Id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CaptainUserId).HasColumnName("CaptainUser_id");

                entity.Property(e => e.CityName).HasMaxLength(255);

                entity.Property(e => e.TeamName).HasMaxLength(255);
            });

            modelBuilder.Entity<TeamCaptainUser>(entity =>
            {
                entity.HasKey(e => new { e.TeamId, e.UserId })
                    .HasName("aaaaaTeamCaptainUser_PK");

                entity.HasIndex(e => e.TeamId)
                    .HasName("TeamTeamCaptainUser");

                entity.HasIndex(e => e.UserId)
                    .HasName("UserTeamCaptainUser");

                entity.Property(e => e.TeamId).HasColumnName("Team_id");

                entity.Property(e => e.UserId).HasColumnName("User_id");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.TeamCaptainUser)
                    .HasForeignKey(d => d.TeamId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("TeamCaptainUser_FK00");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TeamCaptainUser)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("TeamCaptainUser_FK01");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.DiscordId)
                    .HasName("discord_id")
                    .IsUnique();

                entity.HasIndex(e => e.TeamId)
                    .HasName("TeamUser");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Added)
                    .HasColumnName("added")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.ApiavatarUrl)
                    .HasColumnName("APIAvatarURL")
                    .HasMaxLength(255);

                entity.Property(e => e.ApicurrentSr)
                    .HasColumnName("APICurrentSR")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.ApideathAvg)
                    .HasColumnName("APIDeathAvg")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.ApihealAvg)
                    .HasColumnName("APIHealAvg")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Apihero1)
                    .HasColumnName("APIHero1")
                    .HasMaxLength(255);

                entity.Property(e => e.Apihero2)
                    .HasColumnName("APIHero2")
                    .HasMaxLength(255);

                entity.Property(e => e.Apihero3)
                    .HasColumnName("APIHero3")
                    .HasMaxLength(255);

                entity.Property(e => e.ApikillAvg)
                    .HasColumnName("APIKillAvg")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.ApitimePlayed).HasColumnName("APITimePlayed");

                entity.Property(e => e.ApiwinRate).HasColumnName("APIWinRate");

                entity.Property(e => e.Attending).HasColumnName("attending");

                entity.Property(e => e.BtagId).HasMaxLength(255);

                entity.Property(e => e.CurrentSr)
                    .HasColumnName("CurrentSR")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.DiscordId)
                    .IsRequired()
                    .HasColumnName("discord_id")
                    .HasMaxLength(40);

                entity.Property(e => e.Division).HasMaxLength(255);

                entity.Property(e => e.TeamId).HasColumnName("Team_id");

                entity.Property(e => e.Username).HasMaxLength(255);

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.TeamId)
                    .HasConstraintName("User_FK00");
            });
        }
    }
}