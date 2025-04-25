using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_Reclutamiento.Migrations
{
    /// <inheritdoc />
    public partial class firstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Postulantes",
                columns: table => new
                {
                    PostulanteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Postulantes", x => x.PostulanteId);
                });

            migrationBuilder.CreateTable(
                name: "Domicilios",
                columns: table => new
                {
                    DomicilioId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostulanteId = table.Column<int>(type: "int", nullable: false),
                    Calle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ciudad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CodigoPostal = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Domicilios", x => x.DomicilioId);
                    table.ForeignKey(
                        name: "FK_Domicilios_Postulantes_PostulanteId",
                        column: x => x.PostulanteId,
                        principalTable: "Postulantes",
                        principalColumn: "PostulanteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Domicilios_PostulanteId",
                table: "Domicilios",
                column: "PostulanteId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Postulantes_Email",
                table: "Postulantes",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Domicilios");

            migrationBuilder.DropTable(
                name: "Postulantes");
        }
    }
}
