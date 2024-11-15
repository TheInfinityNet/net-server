﻿// <auto-generated />
using System;
using InfinityNetServer.Services.Post.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace InfinityNetServer.Services.Post.Infrastructure.Data.Migrations
{
    [DbContext(typeof(PostDbContext))]
    partial class PostDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("InfinityNetServer.Services.Post.Domain.Entities.Post", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("jsonb")
                        .HasColumnName("content");

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

                    b.Property<Guid?>("FileMetadataId")
                        .HasColumnType("uuid")
                        .HasColumnName("file_metadata_id");

                    b.Property<Guid?>("GroupId")
                        .HasColumnType("uuid")
                        .HasColumnName("group_id");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_deleted");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uuid")
                        .HasColumnName("owner_id");

                    b.Property<Guid?>("ParentId")
                        .HasColumnType("uuid")
                        .HasColumnName("parent_id");

                    b.Property<Guid?>("PresentationId")
                        .HasColumnType("uuid")
                        .HasColumnName("presentation_id");

                    b.Property<int>("Type")
                        .HasColumnType("integer")
                        .HasColumnName("post_type");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("updated_at");

                    b.Property<Guid?>("UpdatedBy")
                        .HasMaxLength(255)
                        .HasColumnType("uuid")
                        .HasColumnName("updated_by");

                    b.HasKey("Id");

                    b.HasIndex("CreatedAt");

                    b.HasIndex("GroupId");

                    b.HasIndex("OwnerId");

                    b.HasIndex("ParentId");

                    b.HasIndex("PresentationId");

                    b.HasIndex("Type");

                    b.ToTable("posts");
                });

            modelBuilder.Entity("InfinityNetServer.Services.Post.Domain.Entities.PostPrivacy", b =>
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

                    b.Property<Guid>("PostId")
                        .HasColumnType("uuid")
                        .HasColumnName("post_id");

                    b.Property<int>("Type")
                        .HasColumnType("integer")
                        .HasColumnName("privacy_type");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("updated_at");

                    b.Property<Guid?>("UpdatedBy")
                        .HasMaxLength(255)
                        .HasColumnType("uuid")
                        .HasColumnName("updated_by");

                    b.HasKey("Id");

                    b.HasIndex("PostId");

                    b.HasIndex("Type");

                    b.ToTable("post_privacies");
                });

            modelBuilder.Entity("InfinityNetServer.Services.Post.Domain.Entities.PostPrivacyExclude", b =>
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

                    b.Property<Guid>("PostPrivacyId")
                        .HasColumnType("uuid")
                        .HasColumnName("post_privacy_id");

                    b.Property<Guid>("ProfileId")
                        .HasColumnType("uuid")
                        .HasColumnName("profile_id");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("updated_at");

                    b.Property<Guid?>("UpdatedBy")
                        .HasMaxLength(255)
                        .HasColumnType("uuid")
                        .HasColumnName("updated_by");

                    b.HasKey("Id");

                    b.HasIndex("PostPrivacyId");

                    b.HasIndex("ProfileId");

                    b.ToTable("post_privacy_excludes");
                });

            modelBuilder.Entity("InfinityNetServer.Services.Post.Domain.Entities.PostPrivacyInclude", b =>
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

                    b.Property<Guid>("PostPrivacyId")
                        .HasColumnType("uuid")
                        .HasColumnName("post_privacy_id");

                    b.Property<Guid>("ProfileId")
                        .HasColumnType("uuid")
                        .HasColumnName("profile_id");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("updated_at");

                    b.Property<Guid?>("UpdatedBy")
                        .HasMaxLength(255)
                        .HasColumnType("uuid")
                        .HasColumnName("updated_by");

                    b.HasKey("Id");

                    b.HasIndex("PostPrivacyId");

                    b.HasIndex("ProfileId");

                    b.ToTable("post_privacy_includes");
                });

            modelBuilder.Entity("InfinityNetServer.Services.Post.Domain.Entities.Post", b =>
                {
                    b.HasOne("InfinityNetServer.Services.Post.Domain.Entities.Post", "Parent")
                        .WithMany("SharedPosts")
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("InfinityNetServer.Services.Post.Domain.Entities.Post", "Presentation")
                        .WithMany("SubPosts")
                        .HasForeignKey("PresentationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Parent");

                    b.Navigation("Presentation");
                });

            modelBuilder.Entity("InfinityNetServer.Services.Post.Domain.Entities.PostPrivacy", b =>
                {
                    b.HasOne("InfinityNetServer.Services.Post.Domain.Entities.Post", "Post")
                        .WithMany("PostPrivacies")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Post");
                });

            modelBuilder.Entity("InfinityNetServer.Services.Post.Domain.Entities.PostPrivacyExclude", b =>
                {
                    b.HasOne("InfinityNetServer.Services.Post.Domain.Entities.PostPrivacy", "PostPrivacy")
                        .WithMany("PostPrivacyExcludes")
                        .HasForeignKey("PostPrivacyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PostPrivacy");
                });

            modelBuilder.Entity("InfinityNetServer.Services.Post.Domain.Entities.PostPrivacyInclude", b =>
                {
                    b.HasOne("InfinityNetServer.Services.Post.Domain.Entities.PostPrivacy", "PostPrivacy")
                        .WithMany("PostPrivacyIncludes")
                        .HasForeignKey("PostPrivacyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PostPrivacy");
                });

            modelBuilder.Entity("InfinityNetServer.Services.Post.Domain.Entities.Post", b =>
                {
                    b.Navigation("PostPrivacies");

                    b.Navigation("SharedPosts");

                    b.Navigation("SubPosts");
                });

            modelBuilder.Entity("InfinityNetServer.Services.Post.Domain.Entities.PostPrivacy", b =>
                {
                    b.Navigation("PostPrivacyExcludes");

                    b.Navigation("PostPrivacyIncludes");
                });
#pragma warning restore 612, 618
        }
    }
}
