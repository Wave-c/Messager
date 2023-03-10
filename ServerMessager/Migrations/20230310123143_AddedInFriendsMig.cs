using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerMessager.Migrations
{
    /// <inheritdoc />
    public partial class AddedInFriendsMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AddedInFriends",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    User1 = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    User2 = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddedInFriends", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AddedInFriends");
        }
    }
}
