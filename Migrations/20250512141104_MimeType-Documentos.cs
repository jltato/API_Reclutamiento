using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_Reclutamiento.Migrations
{
    /// <inheritdoc />
    public partial class MimeTypeDocumentos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MIME",
                table: "Documentos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MIME",
                table: "Documentos");
        }
    }
}
