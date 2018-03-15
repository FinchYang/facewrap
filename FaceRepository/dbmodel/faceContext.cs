﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FaceRepository.dbmodel
{
    public partial class faceContext : DbContext
    {
        public virtual DbSet<Person> Person { get; set; }
        public virtual DbSet<Trails> Trails { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySql("Server=localhost;port=3306;User Id=root;Password=root;Database=face");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>(entity =>
            {
                entity.HasKey(e => e.Idperson);

                entity.ToTable("person");

                entity.HasIndex(e => e.Idcardno)
                    .HasName("idcardno_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.Idperson)
                    .HasName("idperson_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Idperson)
                    .HasColumnName("idperson")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasColumnName("address")
                    .HasMaxLength(200);

                entity.Property(e => e.Birthday)
                    .IsRequired()
                    .HasColumnName("birthday")
                    .HasMaxLength(10);

                entity.Property(e => e.Enddate)
                    .IsRequired()
                    .HasColumnName("enddate")
                    .HasMaxLength(45);

                entity.Property(e => e.Gender)
                    .IsRequired()
                    .HasColumnName("gender")
                    .HasMaxLength(10);

                entity.Property(e => e.Idcardno)
                    .IsRequired()
                    .HasColumnName("idcardno")
                    .HasMaxLength(45);

                entity.Property(e => e.Info)
                    .IsRequired()
                    .HasColumnName("info");

                entity.Property(e => e.Issuer)
                    .IsRequired()
                    .HasColumnName("issuer")
                    .HasMaxLength(45);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(45);

                entity.Property(e => e.Nation)
                    .IsRequired()
                    .HasColumnName("nation")
                    .HasMaxLength(45)
                    .HasDefaultValueSql("'民族'");

                entity.Property(e => e.Nationality)
                    .IsRequired()
                    .HasColumnName("nationality")
                    .HasMaxLength(45)
                    .HasDefaultValueSql("'国籍'");

                entity.Property(e => e.Startdate)
                    .IsRequired()
                    .HasColumnName("startdate")
                    .HasMaxLength(45);
            });

            modelBuilder.Entity<Trails>(entity =>
            {
                entity.HasKey(e => e.Idtrails);

                entity.ToTable("trails");

                entity.HasIndex(e => e.Idtrails)
                    .HasName("idtrails_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Idtrails)
                    .HasColumnName("idtrails")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasColumnName("address")
                    .HasMaxLength(200);

                entity.Property(e => e.Idcardno)
                    .IsRequired()
                    .HasColumnName("idcardno")
                    .HasMaxLength(45);

                entity.Property(e => e.Info)
                    .IsRequired()
                    .HasColumnName("info");

                entity.Property(e => e.Operatingagency)
                    .IsRequired()
                    .HasColumnName("operatingagency")
                    .HasMaxLength(200);

                entity.Property(e => e.TimeStamp)
                    .HasColumnName("time_stamp")
                    .HasColumnType("datetime");
            });
        }
    }
}