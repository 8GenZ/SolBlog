using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolBlog.Data.Migrations
{
    /// <inheritdoc />
    public partial class _004_initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BlogUserId",
                schema: "blog",
                table: "Categories");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BlogUserId",
                schema: "blog",
                table: "Categories",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
