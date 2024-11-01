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
                name: "interactions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    profile_id = table.Column<Guid>(type: "uuid", nullable: false),
                    relate_profile_id = table.Column<Guid>(type: "uuid", nullable: false),
                    friendship_id = table.Column<Guid>(type: "uuid", nullable: true),
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
                    table.PrimaryKey("PK_interactions", x => x.id);
                    table.ForeignKey(
                        name: "FK_interactions_friendships_friendship_id",
                        column: x => x.friendship_id,
                        principalTable: "friendships",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
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
                name: "IX_interactions_created_at",
                table: "interactions",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "IX_interactions_friendship_id",
                table: "interactions",
                column: "friendship_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_interactions_profile_id_relate_profile_id",
                table: "interactions",
                columns: new[] { "profile_id", "relate_profile_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_interactions_type",
                table: "interactions",
                column: "type");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "interactions");

            migrationBuilder.DropTable(
                name: "friendships");
        }
    }
}
