﻿// <auto-generated />
using System;
using InfinityNetServer.Services.Comment.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace InfinityNetServer.Services.Comment.Infrastructure.Data.Migrations
{
    [DbContext(typeof(CommentDbContext))]
    partial class CommentDbContextModelSnapshot : ModelSnapshot
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

            modelBuilder.Entity("InfinityNetServer.Services.Comment.Domain.Entities.Comment", b =>
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

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_deleted");

                    b.Property<Guid?>("ParentId")
                        .HasColumnType("uuid")
                        .HasColumnName("parent_id");

                    b.Property<Guid>("PostId")
                        .HasColumnType("uuid")
                        .HasColumnName("post_id");

                    b.Property<Guid>("ProfileId")
                        .HasColumnType("uuid")
                        .HasColumnName("profile_id");

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

                    b.HasIndex("CreatedAt");

                    b.HasIndex("ParentId");

                    b.HasIndex("PostId");

                    b.HasIndex("ProfileId");

                    b.HasIndex("Type");

                    b.ToTable("comments");
                });

            modelBuilder.Entity("InfinityNetServer.Services.Comment.Domain.Entities.Comment", b =>
                {
                    b.HasOne("InfinityNetServer.Services.Comment.Domain.Entities.Comment", "ParentComment")
                        .WithMany("RepliesComments")
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("ParentComment");
                });

            modelBuilder.Entity("InfinityNetServer.Services.Comment.Domain.Entities.Comment", b =>
                {
                    b.Navigation("RepliesComments");
                });
#pragma warning restore 612, 618
        }
    }
}
