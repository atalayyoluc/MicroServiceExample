using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCustomerTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tel",
                table: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Tel",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
