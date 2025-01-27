using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleManagerAPI.Migrations
{
    /// <inheritdoc />
    public partial class ChangeLastUpdatedColumnstoLastModifiedColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastUpdatedDate",
                table: "Note",
                newName: "LastModifiedDate");

            migrationBuilder.RenameColumn(
                name: "LastUpdatedBy",
                table: "Note",
                newName: "LastModifiedBy");

            migrationBuilder.RenameColumn(
                name: "LastUpdatedDate",
                table: "MessageTemplate",
                newName: "LastModifiedDate");

            migrationBuilder.RenameColumn(
                name: "LastUpdatedBy",
                table: "MessageTemplate",
                newName: "LastModifiedBy");

            migrationBuilder.RenameColumn(
                name: "LastUpdatedDate",
                table: "Message",
                newName: "LastModifiedDate");

            migrationBuilder.RenameColumn(
                name: "LastUpdatedBy",
                table: "Message",
                newName: "LastModifiedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastModifiedDate",
                table: "Note",
                newName: "LastUpdatedDate");

            migrationBuilder.RenameColumn(
                name: "LastModifiedBy",
                table: "Note",
                newName: "LastUpdatedBy");

            migrationBuilder.RenameColumn(
                name: "LastModifiedDate",
                table: "MessageTemplate",
                newName: "LastUpdatedDate");

            migrationBuilder.RenameColumn(
                name: "LastModifiedBy",
                table: "MessageTemplate",
                newName: "LastUpdatedBy");

            migrationBuilder.RenameColumn(
                name: "LastModifiedDate",
                table: "Message",
                newName: "LastUpdatedDate");

            migrationBuilder.RenameColumn(
                name: "LastModifiedBy",
                table: "Message",
                newName: "LastUpdatedBy");
        }
    }
}
