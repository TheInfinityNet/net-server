﻿// <auto-generated />
using System;
using InfinityNetServer.Services.Identity.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace InfinityNetServer.Services.Identity.Infrastructure.Data.Migrations
{
    [DbContext(typeof(IdentityDbContext))]
    [Migration("20241208090903_InitialMigration")]
    partial class InitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("InfinityNetServer.Services.Identity.Domain.Entities.Account", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_at");

                    b.Property<Guid?>("CreatedBy")
                        .HasMaxLength(255)
                        .HasColumnType("uuid")
                        .HasColumnName("created_by");

                    b.Property<Guid>("DefaultUserProfileId")
                        .HasColumnType("uuid")
                        .HasColumnName("default_user_profile_id");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("deleted_at");

                    b.Property<Guid?>("DeletedBy")
                        .HasMaxLength(255)
                        .HasColumnType("uuid")
                        .HasColumnName("deleted_by");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_deleted");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("updated_at");

                    b.Property<Guid?>("UpdatedBy")
                        .HasMaxLength(255)
                        .HasColumnType("uuid")
                        .HasColumnName("updated_by");

                    b.HasKey("Id");

                    b.HasIndex("DefaultUserProfileId")
                        .IsUnique();

                    b.ToTable("accounts");
                });

            modelBuilder.Entity("InfinityNetServer.Services.Identity.Domain.Entities.AccountProvider", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("uuid")
                        .HasColumnName("account_id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_at");

                    b.Property<Guid?>("CreatedBy")
                        .HasMaxLength(255)
                        .HasColumnType("uuid")
                        .HasColumnName("created_by");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("deleted_at");

                    b.Property<Guid?>("DeletedBy")
                        .HasMaxLength(255)
                        .HasColumnType("uuid")
                        .HasColumnName("deleted_by");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_deleted");

                    b.Property<int>("Type")
                        .HasColumnType("integer")
                        .HasColumnName("type");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("updated_at");

                    b.Property<Guid?>("UpdatedBy")
                        .HasMaxLength(255)
                        .HasColumnType("uuid")
                        .HasColumnName("updated_by");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.ToTable("account_providers");

                    b.UseTptMappingStrategy();
                });

            modelBuilder.Entity("InfinityNetServer.Services.Identity.Domain.Entities.Verification", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("uuid")
                        .HasColumnName("account_id");

                    b.Property<string>("Code")
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)")
                        .HasColumnName("code");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_at");

                    b.Property<Guid?>("CreatedBy")
                        .HasMaxLength(255)
                        .HasColumnType("uuid")
                        .HasColumnName("created_by");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("deleted_at");

                    b.Property<Guid?>("DeletedBy")
                        .HasMaxLength(255)
                        .HasColumnType("uuid")
                        .HasColumnName("deleted_by");

                    b.Property<DateTime>("ExpiresAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("expires_at");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_deleted");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("token");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("updated_at");

                    b.Property<Guid?>("UpdatedBy")
                        .HasMaxLength(255)
                        .HasColumnType("uuid")
                        .HasColumnName("updated_by");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.HasIndex("Token")
                        .IsUnique();

                    b.ToTable("verifications");
                });

            modelBuilder.Entity("InfinityNetServer.Services.Identity.Domain.Entities.ExternalProvider", b =>
                {
                    b.HasBaseType("InfinityNetServer.Services.Identity.Domain.Entities.AccountProvider");

                    b.Property<int>("ExternalName")
                        .HasColumnType("integer")
                        .HasColumnName("external_name");

                    b.Property<string>("UserId")
                        .HasColumnType("text")
                        .HasColumnName("user_id");

                    b.HasIndex("ExternalName");

                    b.HasIndex("UserId");

                    b.ToTable("external_providers");
                });

            modelBuilder.Entity("InfinityNetServer.Services.Identity.Domain.Entities.LocalProvider", b =>
                {
                    b.HasBaseType("InfinityNetServer.Services.Identity.Domain.Entities.AccountProvider");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("email");

                    b.Property<string>("PasswordHash")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("password_hash");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("local_providers");
                });

            modelBuilder.Entity("InfinityNetServer.Services.Identity.Domain.Entities.AccountProvider", b =>
                {
                    b.HasOne("InfinityNetServer.Services.Identity.Domain.Entities.Account", "Account")
                        .WithMany("AccountProviders")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("InfinityNetServer.Services.Identity.Domain.Entities.Verification", b =>
                {
                    b.HasOne("InfinityNetServer.Services.Identity.Domain.Entities.Account", "Account")
                        .WithMany("Verifications")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("InfinityNetServer.Services.Identity.Domain.Entities.ExternalProvider", b =>
                {
                    b.HasOne("InfinityNetServer.Services.Identity.Domain.Entities.AccountProvider", "AccountProvider")
                        .WithOne("GoogleProvider")
                        .HasForeignKey("InfinityNetServer.Services.Identity.Domain.Entities.ExternalProvider", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AccountProvider");
                });

            modelBuilder.Entity("InfinityNetServer.Services.Identity.Domain.Entities.LocalProvider", b =>
                {
                    b.HasOne("InfinityNetServer.Services.Identity.Domain.Entities.AccountProvider", "AccountProvider")
                        .WithOne("LocalProvider")
                        .HasForeignKey("InfinityNetServer.Services.Identity.Domain.Entities.LocalProvider", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AccountProvider");
                });

            modelBuilder.Entity("InfinityNetServer.Services.Identity.Domain.Entities.Account", b =>
                {
                    b.Navigation("AccountProviders");

                    b.Navigation("Verifications");
                });

            modelBuilder.Entity("InfinityNetServer.Services.Identity.Domain.Entities.AccountProvider", b =>
                {
                    b.Navigation("GoogleProvider");

                    b.Navigation("LocalProvider");
                });
#pragma warning restore 612, 618
        }
    }
}
