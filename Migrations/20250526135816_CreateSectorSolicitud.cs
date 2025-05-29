using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_Reclutamiento.Migrations
{
    /// <inheritdoc />
    public partial class CreateSectorSolicitud : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SectorSolicitudId",
                table: "Seguimientos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "NoApto",
                table: "EstadoSeguimientos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "SectorSolicitud",
                columns: table => new
                {
                    SectorSolicitudId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreSector = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SectorSolicitud", x => x.SectorSolicitudId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Seguimientos_SectorSolicitudId",
                table: "Seguimientos",
                column: "SectorSolicitudId");

            migrationBuilder.AddForeignKey(
                name: "FK_Seguimientos_SectorSolicitud_SectorSolicitudId",
                table: "Seguimientos",
                column: "SectorSolicitudId",
                principalTable: "SectorSolicitud",
                principalColumn: "SectorSolicitudId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Seguimientos_SectorSolicitud_SectorSolicitudId",
                table: "Seguimientos");

            migrationBuilder.DropTable(
                name: "SectorSolicitud");

            migrationBuilder.DropIndex(
                name: "IX_Seguimientos_SectorSolicitudId",
                table: "Seguimientos");

            migrationBuilder.DropColumn(
                name: "SectorSolicitudId",
                table: "Seguimientos");

            migrationBuilder.DropColumn(
                name: "NoApto",
                table: "EstadoSeguimientos");
        }
    }
}
