using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_Reclutamiento.Migrations
{
    /// <inheritdoc />
    public partial class pruebaNavegacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Postulantes_Email",
                table: "Postulantes");

            migrationBuilder.DropIndex(
                name: "IX_Domicilios_PostulanteId",
                table: "Domicilios");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Postulantes");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Postulantes",
                newName: "Nombre");

            migrationBuilder.RenameColumn(
                name: "Ciudad",
                table: "Domicilios",
                newName: "Numero");

            migrationBuilder.AddColumn<DateOnly>(
                name: "FechaNac",
                table: "Postulantes",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<int>(
                name: "IdNacionalidad",
                table: "Postulantes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdLocalidad",
                table: "Domicilios",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Nacionalidades",
                columns: table => new
                {
                    IdNacionalidad = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreNacionalidad = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nacionalidades", x => x.IdNacionalidad);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Postulantes_IdNacionalidad",
                table: "Postulantes",
                column: "IdNacionalidad");

            migrationBuilder.CreateIndex(
                name: "IX_Domicilios_PostulanteId",
                table: "Domicilios",
                column: "PostulanteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Postulantes_Nacionalidades_IdNacionalidad",
                table: "Postulantes",
                column: "IdNacionalidad",
                principalTable: "Nacionalidades",
                principalColumn: "IdNacionalidad",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Postulantes_Nacionalidades_IdNacionalidad",
                table: "Postulantes");

            migrationBuilder.DropTable(
                name: "Nacionalidades");

            migrationBuilder.DropIndex(
                name: "IX_Postulantes_IdNacionalidad",
                table: "Postulantes");

            migrationBuilder.DropIndex(
                name: "IX_Domicilios_PostulanteId",
                table: "Domicilios");

            migrationBuilder.DropColumn(
                name: "FechaNac",
                table: "Postulantes");

            migrationBuilder.DropColumn(
                name: "IdNacionalidad",
                table: "Postulantes");

            migrationBuilder.DropColumn(
                name: "IdLocalidad",
                table: "Domicilios");

            migrationBuilder.RenameColumn(
                name: "Nombre",
                table: "Postulantes",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Numero",
                table: "Domicilios",
                newName: "Ciudad");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Postulantes",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Postulantes_Email",
                table: "Postulantes",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Domicilios_PostulanteId",
                table: "Domicilios",
                column: "PostulanteId",
                unique: true);
        }
    }
}
