using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_Reclutamiento.Migrations
{
    /// <inheritdoc />
    public partial class TablaEstadoSeguimiento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Postulantes_establecimientos_EstabSolicitudId",
                table: "Postulantes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_establecimientos",
                table: "establecimientos");

            migrationBuilder.DropColumn(
                name: "Antiguedad",
                table: "Trabajo");

            migrationBuilder.DropColumn(
                name: "AptoGrupal",
                table: "Seguimientos");

            migrationBuilder.DropColumn(
                name: "AptoMedico",
                table: "Seguimientos");

            migrationBuilder.DropColumn(
                name: "AptoPsicologico",
                table: "Seguimientos");

            migrationBuilder.DropColumn(
                name: "AptoReconocMed",
                table: "Seguimientos");

            migrationBuilder.DropColumn(
                name: "EntrevistaGrupal",
                table: "Seguimientos");

            migrationBuilder.DropColumn(
                name: "EntrevistaPsicologica",
                table: "Seguimientos");

            migrationBuilder.DropColumn(
                name: "Notificado",
                table: "Seguimientos");

            migrationBuilder.DropColumn(
                name: "ReconocimientoMed",
                table: "Seguimientos");

            migrationBuilder.DropColumn(
                name: "TurnoMedico",
                table: "Seguimientos");

            migrationBuilder.RenameTable(
                name: "establecimientos",
                newName: "Establecimientos");

            migrationBuilder.AddColumn<DateOnly>(
                name: "Desde",
                table: "Trabajo",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateOnly>(
                name: "Hasta",
                table: "Trabajo",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<int>(
                name: "EstadoSeguimientoActualId",
                table: "Seguimientos",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Establecimientos",
                table: "Establecimientos",
                column: "EstablecimientoId");

            migrationBuilder.CreateTable(
                name: "EtapaSeguimientos",
                columns: table => new
                {
                    EtapaSeguimientoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreEtapa = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EtapaSeguimientos", x => x.EtapaSeguimientoId);
                });

            migrationBuilder.CreateTable(
                name: "EstadoSeguimientos",
                columns: table => new
                {
                    EstadoSeguimientoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeguimientoId = table.Column<int>(type: "int", nullable: false),
                    EtapaSeguimientoId = table.Column<int>(type: "int", nullable: false),
                    FechaTurno = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Asistencia = table.Column<bool>(type: "bit", nullable: false),
                    Apto = table.Column<bool>(type: "bit", nullable: false),
                    Notificado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadoSeguimientos", x => x.EstadoSeguimientoId);
                    table.ForeignKey(
                        name: "FK_EstadoSeguimientos_EtapaSeguimientos_EtapaSeguimientoId",
                        column: x => x.EtapaSeguimientoId,
                        principalTable: "EtapaSeguimientos",
                        principalColumn: "EtapaSeguimientoId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EstadoSeguimientos_Seguimientos_SeguimientoId",
                        column: x => x.SeguimientoId,
                        principalTable: "Seguimientos",
                        principalColumn: "SeguimientoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Seguimientos_EstadoSeguimientoActualId",
                table: "Seguimientos",
                column: "EstadoSeguimientoActualId");

            migrationBuilder.CreateIndex(
                name: "IX_EstadoSeguimientos_EtapaSeguimientoId",
                table: "EstadoSeguimientos",
                column: "EtapaSeguimientoId");

            migrationBuilder.CreateIndex(
                name: "IX_EstadoSeguimientos_SeguimientoId",
                table: "EstadoSeguimientos",
                column: "SeguimientoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Postulantes_Establecimientos_EstabSolicitudId",
                table: "Postulantes",
                column: "EstabSolicitudId",
                principalTable: "Establecimientos",
                principalColumn: "EstablecimientoId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Seguimientos_EstadoSeguimientos_EstadoSeguimientoActualId",
                table: "Seguimientos",
                column: "EstadoSeguimientoActualId",
                principalTable: "EstadoSeguimientos",
                principalColumn: "EstadoSeguimientoId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Postulantes_Establecimientos_EstabSolicitudId",
                table: "Postulantes");

            migrationBuilder.DropForeignKey(
                name: "FK_Seguimientos_EstadoSeguimientos_EstadoSeguimientoActualId",
                table: "Seguimientos");

            migrationBuilder.DropTable(
                name: "EstadoSeguimientos");

            migrationBuilder.DropTable(
                name: "EtapaSeguimientos");

            migrationBuilder.DropIndex(
                name: "IX_Seguimientos_EstadoSeguimientoActualId",
                table: "Seguimientos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Establecimientos",
                table: "Establecimientos");

            migrationBuilder.DropColumn(
                name: "Desde",
                table: "Trabajo");

            migrationBuilder.DropColumn(
                name: "Hasta",
                table: "Trabajo");

            migrationBuilder.DropColumn(
                name: "EstadoSeguimientoActualId",
                table: "Seguimientos");

            migrationBuilder.RenameTable(
                name: "Establecimientos",
                newName: "establecimientos");

            migrationBuilder.AddColumn<int>(
                name: "Antiguedad",
                table: "Trabajo",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "AptoGrupal",
                table: "Seguimientos",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AptoMedico",
                table: "Seguimientos",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AptoPsicologico",
                table: "Seguimientos",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AptoReconocMed",
                table: "Seguimientos",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "EntrevistaGrupal",
                table: "Seguimientos",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateOnly>(
                name: "EntrevistaPsicologica",
                table: "Seguimientos",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<bool>(
                name: "Notificado",
                table: "Seguimientos",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "ReconocimientoMed",
                table: "Seguimientos",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateOnly>(
                name: "TurnoMedico",
                table: "Seguimientos",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddPrimaryKey(
                name: "PK_establecimientos",
                table: "establecimientos",
                column: "EstablecimientoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Postulantes_establecimientos_EstabSolicitudId",
                table: "Postulantes",
                column: "EstabSolicitudId",
                principalTable: "establecimientos",
                principalColumn: "EstablecimientoId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
