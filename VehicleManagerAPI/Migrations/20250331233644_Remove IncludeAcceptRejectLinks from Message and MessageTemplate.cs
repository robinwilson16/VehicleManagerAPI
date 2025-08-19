using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleManagerAPI.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIncludeAcceptRejectLinksfromMessageandMessageTemplate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IncludeAcceptRejectLinks",
                table: "MessageTemplate");

            migrationBuilder.DropColumn(
                name: "IncludeAcceptRejectLinks",
                table: "Message");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IncludeAcceptRejectLinks",
                table: "MessageTemplate",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IncludeAcceptRejectLinks",
                table: "Message",
                type: "bit",
                nullable: true);
        }
    }
}
