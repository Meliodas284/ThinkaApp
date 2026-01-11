using Microsoft.EntityFrameworkCore.Migrations;
using NpgsqlTypes;

#nullable disable

namespace Thinka.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddSearchVectorToIdeas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<NpgsqlTsVector>(
                name: "SearchVector",
                table: "Ideas",
                type: "tsvector",
                nullable: false,
                computedColumnSql: "setweight(to_tsvector('russian', \"Title\"), 'A') || setweight(to_tsvector('russian', \"ShortDescription\"), 'B') || setweight(to_tsvector('russian', \"FullDescription\"), 'C')",
                stored: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ideas_SearchVector",
                table: "Ideas",
                column: "SearchVector")
                .Annotation("Npgsql:IndexMethod", "GIN");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Ideas_SearchVector",
                table: "Ideas");

            migrationBuilder.DropColumn(
                name: "SearchVector",
                table: "Ideas");
        }
    }
}
