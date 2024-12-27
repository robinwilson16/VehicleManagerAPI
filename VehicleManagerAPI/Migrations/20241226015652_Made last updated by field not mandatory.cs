using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleManagerAPI.Migrations
{
    /// <inheritdoc />
    public partial class Madelastupdatedbyfieldnotmandatory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Template",
                table: "MessageTemplate",
                newName: "TemplateContent");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TemplateContent",
                table: "MessageTemplate",
                newName: "Template");
        }
    }
}
