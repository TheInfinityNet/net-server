using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfinityNetServer.Services.Post.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "posts",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    content = table.Column<string>(type: "jsonb", nullable: false),
                    post_type = table.Column<int>(type: "integer", nullable: false),
                    presentation_id = table.Column<Guid>(type: "uuid", nullable: true),
                    parent_id = table.Column<Guid>(type: "uuid", nullable: true),
                    owner_id = table.Column<Guid>(type: "uuid", nullable: false),
                    group_id = table.Column<Guid>(type: "uuid", nullable: true),
                    file_metadata_id = table.Column<Guid>(type: "uuid", nullable: true),
                    created_by = table.Column<Guid>(type: "uuid", maxLength: 255, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_by = table.Column<Guid>(type: "uuid", maxLength: 255, nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    deleted_by = table.Column<Guid>(type: "uuid", maxLength: 255, nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_posts", x => x.id);
                    table.ForeignKey(
                        name: "FK_posts_posts_parent_id",
                        column: x => x.parent_id,
                        principalTable: "posts",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_posts_posts_presentation_id",
                        column: x => x.presentation_id,
                        principalTable: "posts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "post_privacies",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    post_id = table.Column<Guid>(type: "uuid", nullable: false),
                    privacy_type = table.Column<int>(type: "integer", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", maxLength: 255, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_by = table.Column<Guid>(type: "uuid", maxLength: 255, nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    deleted_by = table.Column<Guid>(type: "uuid", maxLength: 255, nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_post_privacies", x => x.id);
                    table.ForeignKey(
                        name: "FK_post_privacies_posts_post_id",
                        column: x => x.post_id,
                        principalTable: "posts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "post_privacy_excludes",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    post_privacy_id = table.Column<Guid>(type: "uuid", nullable: false),
                    profile_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", maxLength: 255, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_by = table.Column<Guid>(type: "uuid", maxLength: 255, nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    deleted_by = table.Column<Guid>(type: "uuid", maxLength: 255, nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_post_privacy_excludes", x => x.id);
                    table.ForeignKey(
                        name: "FK_post_privacy_excludes_post_privacies_post_privacy_id",
                        column: x => x.post_privacy_id,
                        principalTable: "post_privacies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "post_privacy_includes",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    post_privacy_id = table.Column<Guid>(type: "uuid", nullable: false),
                    profile_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", maxLength: 255, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_by = table.Column<Guid>(type: "uuid", maxLength: 255, nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    deleted_by = table.Column<Guid>(type: "uuid", maxLength: 255, nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_post_privacy_includes", x => x.id);
                    table.ForeignKey(
                        name: "FK_post_privacy_includes_post_privacies_post_privacy_id",
                        column: x => x.post_privacy_id,
                        principalTable: "post_privacies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_post_privacies_post_id",
                table: "post_privacies",
                column: "post_id");

            migrationBuilder.CreateIndex(
                name: "IX_post_privacies_privacy_type",
                table: "post_privacies",
                column: "privacy_type");

            migrationBuilder.CreateIndex(
                name: "IX_post_privacy_excludes_post_privacy_id",
                table: "post_privacy_excludes",
                column: "post_privacy_id");

            migrationBuilder.CreateIndex(
                name: "IX_post_privacy_excludes_profile_id",
                table: "post_privacy_excludes",
                column: "profile_id");

            migrationBuilder.CreateIndex(
                name: "IX_post_privacy_includes_post_privacy_id",
                table: "post_privacy_includes",
                column: "post_privacy_id");

            migrationBuilder.CreateIndex(
                name: "IX_post_privacy_includes_profile_id",
                table: "post_privacy_includes",
                column: "profile_id");

            migrationBuilder.CreateIndex(
                name: "IX_posts_created_at",
                table: "posts",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "IX_posts_group_id",
                table: "posts",
                column: "group_id");

            migrationBuilder.CreateIndex(
                name: "IX_posts_owner_id",
                table: "posts",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "IX_posts_parent_id",
                table: "posts",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "IX_posts_post_type",
                table: "posts",
                column: "post_type");

            migrationBuilder.CreateIndex(
                name: "IX_posts_presentation_id",
                table: "posts",
                column: "presentation_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "post_privacy_excludes");

            migrationBuilder.DropTable(
                name: "post_privacy_includes");

            migrationBuilder.DropTable(
                name: "post_privacies");

            migrationBuilder.DropTable(
                name: "posts");
        }
    }
}
