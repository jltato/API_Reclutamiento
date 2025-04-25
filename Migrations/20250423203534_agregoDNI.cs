using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_Reclutamiento.Migrations
{
    /// <inheritdoc />
    public partial class agregoDNI : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Postulantes_Apellido_Nombre",
                table: "Postulantes");

            migrationBuilder.AddColumn<int>(
                name: "Dni",
                table: "Postulantes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Postulantes_Apellido_Nombre_Dni",
                table: "Postulantes",
                columns: new[] { "Apellido", "Nombre", "Dni" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Postulantes_Apellido_Nombre_Dni",
                table: "Postulantes");

            migrationBuilder.DropColumn(
                name: "Dni",
                table: "Postulantes");

            migrationBuilder.CreateIndex(
                name: "IX_Postulantes_Apellido_Nombre",
                table: "Postulantes",
                columns: new[] { "Apellido", "Nombre" });
        }
    }
}
