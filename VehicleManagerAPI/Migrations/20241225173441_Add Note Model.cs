using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleManagerAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddNoteModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VehicleID",
                table: "Message");

            migrationBuilder.CreateTable(
                name: "Note",
                columns: table => new
                {
                    NoteID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NoteText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAlert = table.Column<bool>(type: "bit", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Note", x => x.NoteID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Message_MessageTemplateID",
                table: "Message",
                column: "MessageTemplateID");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_MessageTemplate_MessageTemplateID",
                table: "Message",
                column: "MessageTemplateID",
                principalTable: "MessageTemplate",
                principalColumn: "MessageTemplateID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_MessageTemplate_MessageTemplateID",
                table: "Message");

            migrationBuilder.DropTable(
                name: "Note");

            migrationBuilder.DropIndex(
                name: "IX_Message_MessageTemplateID",
                table: "Message");

            migrationBuilder.AddColumn<int>(
                name: "VehicleID",
                table: "Message",
                type: "int",
                nullable: true);
        }
    }
}
