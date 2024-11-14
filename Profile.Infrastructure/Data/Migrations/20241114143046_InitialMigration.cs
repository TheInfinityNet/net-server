using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfinityNetServer.Services.Profile.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "profiles",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    account_id = table.Column<Guid>(type: "uuid", nullable: false),
                    avatar_id = table.Column<Guid>(type: "uuid", nullable: true),
                    cover_id = table.Column<Guid>(type: "uuid", nullable: true),
                    mobile_number = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    location = table.Column<string>(type: "text", nullable: true),
                    IsMobileNumberVerified = table.Column<bool>(type: "boolean", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    last_online_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
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
                    table.PrimaryKey("PK_profiles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "page_profiles",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_page_profiles", x => x.id);
                    table.ForeignKey(
                        name: "FK_page_profiles_profiles_id",
                        column: x => x.id,
                        principalTable: "profiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_profiles",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    username = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    first_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    middle_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    last_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    birthdate = table.Column<DateOnly>(type: "date", nullable: false),
                    gender = table.Column<int>(type: "integer", maxLength: 20, nullable: false),
                    bio = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_profiles", x => x.id);
                    table.ForeignKey(
                        name: "FK_user_profiles_profiles_id",
                        column: x => x.id,
                        principalTable: "profiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_page_profiles_name",
                table: "page_profiles",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_profiles_account_id",
                table: "profiles",
                column: "account_id");

            migrationBuilder.CreateIndex(
                name: "IX_profiles_mobile_number",
                table: "profiles",
                column: "mobile_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_profiles_status",
                table: "profiles",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_profiles_type",
                table: "profiles",
                column: "type");

            migrationBuilder.CreateIndex(
                name: "IX_user_profiles_username",
                table: "user_profiles",
                column: "username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "page_profiles");

            migrationBuilder.DropTable(
                name: "user_profiles");

            migrationBuilder.DropTable(
                name: "profiles");
        }
    }
}
