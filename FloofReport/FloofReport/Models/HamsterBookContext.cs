﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FloofReport.Models
{
    public partial class HamsterBookContext : DbContext
    {
        public HamsterBookContext()
        {
        }

        public HamsterBookContext(DbContextOptions<HamsterBookContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Animal> Animals { get; set; }
        public virtual DbSet<Cage> Cages { get; set; }
        public virtual DbSet<Cagearea> Cageareas { get; set; }
        public virtual DbSet<Visualevent> Visualevents { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("Password=Flafik,456;Username=postgres;Database=HamsterBook;Host=mulawa.ddns.net");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Animal>(entity =>
            {
                entity.ToTable("animals");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Dateofadoption).HasColumnName("dateofadoption");

                entity.Property(e => e.Dateofbirth).HasColumnName("dateofbirth");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("name");

                entity.Property(e => e.Race)
                    .HasMaxLength(32)
                    .HasColumnName("race");

                entity.Property(e => e.Sex)
                    .HasColumnType("bit(1)")
                    .HasColumnName("sex");
            });

            modelBuilder.Entity<Cage>(entity =>
            {
                entity.ToTable("cages");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Beddingdepth).HasColumnName("beddingdepth");

                entity.Property(e => e.Beddingtype)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("beddingtype");

                entity.Property(e => e.Location)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasColumnName("location");

                entity.Property(e => e.Size)
                    .HasMaxLength(64)
                    .HasColumnName("size");
            });

            modelBuilder.Entity<Cagearea>(entity =>
            {
                entity.ToTable("cageareas");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Visualevent>(entity =>
            {
                entity.ToTable("visualevents");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Areaid).HasColumnName("areaid");

                entity.Property(e => e.Cageid).HasColumnName("cageid");

                entity.Property(e => e.Hamsterid).HasColumnName("hamsterid");

                entity.Property(e => e.Registrationtime)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("registrationtime");

                entity.HasOne(d => d.Area)
                    .WithMany(p => p.Visualevents)
                    .HasForeignKey(d => d.Areaid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_area");

                entity.HasOne(d => d.Cage)
                    .WithMany(p => p.Visualevents)
                    .HasForeignKey(d => d.Cageid)
                    .HasConstraintName("fk_cage");

                entity.HasOne(d => d.Hamster)
                    .WithMany(p => p.Visualevents)
                    .HasForeignKey(d => d.Hamsterid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_hamster");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}