using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleManagerAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddIncludeAcceptRejectLinksColumntoMessageTemplate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IncludeAcceptRejectLinks",
                table: "MessageTemplate",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IncludeAcceptRejectLinks",
                table: "MessageTemplate");
        }
    }
}
