using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerMessager.Migrations
{
    /// <inheritdoc />
    public partial class ChangeMessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_FromId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_ToId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_FromId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_ToId",
                table: "Messages");

            migrationBuilder.RenameColumn(
                name: "ToId",
                table: "Messages",
                newName: "To");

            migrationBuilder.RenameColumn(
                name: "FromId",
                table: "Messages",
                newName: "From");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "To",
                table: "Messages",
                newName: "ToId");

            migrationBuilder.RenameColumn(
                name: "From",
                table: "Messages",
                newName: "FromId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_FromId",
                table: "Messages",
                column: "FromId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ToId",
                table: "Messages",
                column: "ToId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users_FromId",
                table: "Messages",
                column: "FromId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users_ToId",
                table: "Messages",
                column: "ToId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
