using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DetectiveRoslynIO.Migrations
{
    /// <inheritdoc />
    public partial class AddTrackSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SequenceNumber",
                table: "Challenges",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TrackId",
                table: "Challenges",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ChallengeTracks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    IconClass = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    OrderIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChallengeTracks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserChallengeUnlocks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    ChallengeId = table.Column<int>(type: "INTEGER", nullable: false),
                    UnlockedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsAutoUnlocked = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserChallengeUnlocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserChallengeUnlocks_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserChallengeUnlocks_Challenges_ChallengeId",
                        column: x => x.ChallengeId,
                        principalTable: "Challenges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Challenges_TrackId_SequenceNumber",
                table: "Challenges",
                columns: new[] { "TrackId", "SequenceNumber" });

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeTracks_IsActive",
                table: "ChallengeTracks",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeTracks_OrderIndex",
                table: "ChallengeTracks",
                column: "OrderIndex");

            migrationBuilder.CreateIndex(
                name: "IX_UserChallengeUnlocks_ChallengeId",
                table: "UserChallengeUnlocks",
                column: "ChallengeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserChallengeUnlocks_UnlockedAt",
                table: "UserChallengeUnlocks",
                column: "UnlockedAt");

            migrationBuilder.CreateIndex(
                name: "IX_UserChallengeUnlocks_UserId",
                table: "UserChallengeUnlocks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserChallengeUnlocks_UserId_ChallengeId",
                table: "UserChallengeUnlocks",
                columns: new[] { "UserId", "ChallengeId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Challenges_ChallengeTracks_TrackId",
                table: "Challenges",
                column: "TrackId",
                principalTable: "ChallengeTracks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Challenges_ChallengeTracks_TrackId",
                table: "Challenges");

            migrationBuilder.DropTable(
                name: "ChallengeTracks");

            migrationBuilder.DropTable(
                name: "UserChallengeUnlocks");

            migrationBuilder.DropIndex(
                name: "IX_Challenges_TrackId_SequenceNumber",
                table: "Challenges");

            migrationBuilder.DropColumn(
                name: "SequenceNumber",
                table: "Challenges");

            migrationBuilder.DropColumn(
                name: "TrackId",
                table: "Challenges");
        }
    }
}
