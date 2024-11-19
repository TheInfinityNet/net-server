using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfinityNetServer.Services.Relationship.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "friendships",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    sender_id = table.Column<Guid>(type: "uuid", nullable: false),
                    receiver_id = table.Column<Guid>(type: "uuid", nullable: false),
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
                    table.PrimaryKey("PK_friendships", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "profile_blocks",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    blocker_id = table.Column<Guid>(type: "uuid", nullable: false),
                    blockee_id = table.Column<Guid>(type: "uuid", nullable: false),
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
                    table.PrimaryKey("PK_profile_blocks", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "profile_follows",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    follower_id = table.Column<Guid>(type: "uuid", nullable: false),
                    followee_id = table.Column<Guid>(type: "uuid", nullable: false),
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
                    table.PrimaryKey("PK_profile_follows", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_friendships_created_at",
                table: "friendships",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "IX_friendships_sender_id_receiver_id",
                table: "friendships",
                columns: new[] { "sender_id", "receiver_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_friendships_status",
                table: "friendships",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_profile_blocks_blocker_id_blockee_id",
                table: "profile_blocks",
                columns: new[] { "blocker_id", "blockee_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_profile_blocks_created_at",
                table: "profile_blocks",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "IX_profile_follows_created_at",
                table: "profile_follows",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "IX_profile_follows_follower_id_followee_id",
                table: "profile_follows",
                columns: new[] { "follower_id", "followee_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "friendships");

            migrationBuilder.DropTable(
                name: "profile_blocks");

            migrationBuilder.DropTable(
                name: "profile_follows");
        }
    }
}
