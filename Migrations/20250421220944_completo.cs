using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_Reclutamiento.Migrations
{
    /// <inheritdoc />
    public partial class completo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Postulantes_Nacionalidades_IdNacionalidad",
                table: "Postulantes");

            migrationBuilder.RenameColumn(
                name: "IdNacionalidad",
                table: "Postulantes",
                newName: "SexoId");

            migrationBuilder.RenameIndex(
                name: "IX_Postulantes_IdNacionalidad",
                table: "Postulantes",
                newName: "IX_Postulantes_SexoId");

            migrationBuilder.RenameColumn(
                name: "IdNacionalidad",
                table: "Nacionalidades",
                newName: "NacionalidadId");

            migrationBuilder.RenameColumn(
                name: "IdLocalidad",
                table: "Domicilios",
                newName: "LocalidadId");

            migrationBuilder.AddColumn<int>(
                name: "EstabSolicitudId",
                table: "Postulantes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaSoclicitud",
                table: "Postulantes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Mail",
                table: "Postulantes",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "NacionalidadId",
                table: "Postulantes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Contactos",
                columns: table => new
                {
                    ContactoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostulanteId = table.Column<int>(type: "int", nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Perteneciente = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contactos", x => x.ContactoId);
                    table.ForeignKey(
                        name: "FK_Contactos_Postulantes_PostulanteId",
                        column: x => x.PostulanteId,
                        principalTable: "Postulantes",
                        principalColumn: "PostulanteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "establecimientos",
                columns: table => new
                {
                    EstablecimientoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EstablecimientoNombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EstablecimientoCiudad = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_establecimientos", x => x.EstablecimientoId);
                });

            migrationBuilder.CreateTable(
                name: "Estados",
                columns: table => new
                {
                    EstadoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EstadoNombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estados", x => x.EstadoId);
                });

            migrationBuilder.CreateTable(
                name: "EstadosCiviles",
                columns: table => new
                {
                    EstadoCivilId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EstadoCivilNombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadosCiviles", x => x.EstadoCivilId);
                });

            migrationBuilder.CreateTable(
                name: "Generos",
                columns: table => new
                {
                    GeneroId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GeneroNombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Generos", x => x.GeneroId);
                });

            migrationBuilder.CreateTable(
                name: "Localidades",
                columns: table => new
                {
                    LocalidadId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LocalidadName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Localidades", x => x.LocalidadId);
                });

            migrationBuilder.CreateTable(
                name: "NivelEstudios",
                columns: table => new
                {
                    NivelEstudiosId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NivelNombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NivelEstudios", x => x.NivelEstudiosId);
                });

            migrationBuilder.CreateTable(
                name: "Parentescos",
                columns: table => new
                {
                    ParentescoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentescoNombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parentescos", x => x.ParentescoId);
                });

            migrationBuilder.CreateTable(
                name: "Sexos",
                columns: table => new
                {
                    SexoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SexoName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sexos", x => x.SexoId);
                });

            migrationBuilder.CreateTable(
                name: "TipoDocumentos",
                columns: table => new
                {
                    TipoDocumentoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipoDocumentoNombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoDocumentos", x => x.TipoDocumentoId);
                });

            migrationBuilder.CreateTable(
                name: "TipoInscripcions",
                columns: table => new
                {
                    TipoInscripcionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipoInscripcionNombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoInscripcions", x => x.TipoInscripcionId);
                });

            migrationBuilder.CreateTable(
                name: "Trabajo",
                columns: table => new
                {
                    TrabajoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostulanteId = table.Column<int>(type: "int", nullable: false),
                    ActividadLaboral = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Antiguedad = table.Column<int>(type: "int", nullable: false),
                    IntentoAnterior = table.Column<bool>(type: "bit", nullable: false),
                    EtapaAlzanzada = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OtraFuerza = table.Column<bool>(type: "bit", nullable: false),
                    MotivoBaja = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trabajo", x => x.TrabajoId);
                    table.ForeignKey(
                        name: "FK_Trabajo_Postulantes_PostulanteId",
                        column: x => x.PostulanteId,
                        principalTable: "Postulantes",
                        principalColumn: "PostulanteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DatosPersonales",
                columns: table => new
                {
                    DatosPersonalesId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostulanteId = table.Column<int>(type: "int", nullable: false),
                    GeneroId = table.Column<int>(type: "int", nullable: false),
                    EstadoCivilId = table.Column<int>(type: "int", nullable: false),
                    Altura = table.Column<int>(type: "int", nullable: false),
                    Peso = table.Column<int>(type: "int", nullable: false),
                    TieneTatuajes = table.Column<bool>(type: "bit", nullable: false),
                    CantidadTatuajes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Observaciones = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DatosPersonales", x => x.DatosPersonalesId);
                    table.ForeignKey(
                        name: "FK_DatosPersonales_EstadosCiviles_EstadoCivilId",
                        column: x => x.EstadoCivilId,
                        principalTable: "EstadosCiviles",
                        principalColumn: "EstadoCivilId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DatosPersonales_Generos_GeneroId",
                        column: x => x.GeneroId,
                        principalTable: "Generos",
                        principalColumn: "GeneroId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DatosPersonales_Postulantes_PostulanteId",
                        column: x => x.PostulanteId,
                        principalTable: "Postulantes",
                        principalColumn: "PostulanteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Estudios",
                columns: table => new
                {
                    EstudiosId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostulanteId = table.Column<int>(type: "int", nullable: false),
                    NivelEstudiosId = table.Column<int>(type: "int", nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InstitutoEducativo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EnCurso = table.Column<bool>(type: "bit", nullable: false),
                    AnoEgreso = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estudios", x => x.EstudiosId);
                    table.ForeignKey(
                        name: "FK_Estudios_NivelEstudios_NivelEstudiosId",
                        column: x => x.NivelEstudiosId,
                        principalTable: "NivelEstudios",
                        principalColumn: "NivelEstudiosId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Estudios_Postulantes_PostulanteId",
                        column: x => x.PostulanteId,
                        principalTable: "Postulantes",
                        principalColumn: "PostulanteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Familiares",
                columns: table => new
                {
                    FamiliaresId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostulanteId = table.Column<int>(type: "int", nullable: false),
                    Dni = table.Column<int>(type: "int", nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Convive = table.Column<bool>(type: "bit", nullable: false),
                    ParentescoId = table.Column<int>(type: "int", nullable: false),
                    EsEmpleado = table.Column<bool>(type: "bit", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Familiares", x => x.FamiliaresId);
                    table.ForeignKey(
                        name: "FK_Familiares_Parentescos_ParentescoId",
                        column: x => x.ParentescoId,
                        principalTable: "Parentescos",
                        principalColumn: "ParentescoId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Familiares_Postulantes_PostulanteId",
                        column: x => x.PostulanteId,
                        principalTable: "Postulantes",
                        principalColumn: "PostulanteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Documentos",
                columns: table => new
                {
                    DocumentoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostulanteId = table.Column<int>(type: "int", nullable: false),
                    DocumentoNombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipoDocumentoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documentos", x => x.DocumentoId);
                    table.ForeignKey(
                        name: "FK_Documentos_Postulantes_PostulanteId",
                        column: x => x.PostulanteId,
                        principalTable: "Postulantes",
                        principalColumn: "PostulanteId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Documentos_TipoDocumentos_TipoDocumentoId",
                        column: x => x.TipoDocumentoId,
                        principalTable: "TipoDocumentos",
                        principalColumn: "TipoDocumentoId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Seguimientos",
                columns: table => new
                {
                    SeguimientoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostulanteId = table.Column<int>(type: "int", nullable: false),
                    EstadoId = table.Column<int>(type: "int", nullable: false),
                    TipoInscripcionId = table.Column<int>(type: "int", nullable: false),
                    EntrevistaGrupal = table.Column<DateOnly>(type: "date", nullable: false),
                    AptoGrupal = table.Column<bool>(type: "bit", nullable: true),
                    EntrevistaPsicologica = table.Column<DateOnly>(type: "date", nullable: false),
                    AptoPsicologico = table.Column<bool>(type: "bit", nullable: true),
                    TurnoMedico = table.Column<DateOnly>(type: "date", nullable: false),
                    AptoMedico = table.Column<bool>(type: "bit", nullable: true),
                    ReconocimientoMed = table.Column<DateOnly>(type: "date", nullable: false),
                    AptoReconocMed = table.Column<bool>(type: "bit", nullable: true),
                    Observaciones = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notificado = table.Column<bool>(type: "bit", nullable: true),
                    Modify_At = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Modify_By = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seguimientos", x => x.SeguimientoId);
                    table.ForeignKey(
                        name: "FK_Seguimientos_Estados_EstadoId",
                        column: x => x.EstadoId,
                        principalTable: "Estados",
                        principalColumn: "EstadoId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Seguimientos_Postulantes_PostulanteId",
                        column: x => x.PostulanteId,
                        principalTable: "Postulantes",
                        principalColumn: "PostulanteId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Seguimientos_TipoInscripcions_TipoInscripcionId",
                        column: x => x.TipoInscripcionId,
                        principalTable: "TipoInscripcions",
                        principalColumn: "TipoInscripcionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Postulantes_Apellido_Nombre",
                table: "Postulantes",
                columns: new[] { "Apellido", "Nombre" });

            migrationBuilder.CreateIndex(
                name: "IX_Postulantes_EstabSolicitudId",
                table: "Postulantes",
                column: "EstabSolicitudId");

            migrationBuilder.CreateIndex(
                name: "IX_Postulantes_Mail",
                table: "Postulantes",
                column: "Mail",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Postulantes_NacionalidadId",
                table: "Postulantes",
                column: "NacionalidadId");

            migrationBuilder.CreateIndex(
                name: "IX_Domicilios_LocalidadId",
                table: "Domicilios",
                column: "LocalidadId");

            migrationBuilder.CreateIndex(
                name: "IX_Contactos_PostulanteId",
                table: "Contactos",
                column: "PostulanteId");

            migrationBuilder.CreateIndex(
                name: "IX_DatosPersonales_EstadoCivilId",
                table: "DatosPersonales",
                column: "EstadoCivilId");

            migrationBuilder.CreateIndex(
                name: "IX_DatosPersonales_GeneroId",
                table: "DatosPersonales",
                column: "GeneroId");

            migrationBuilder.CreateIndex(
                name: "IX_DatosPersonales_PostulanteId",
                table: "DatosPersonales",
                column: "PostulanteId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Documentos_PostulanteId",
                table: "Documentos",
                column: "PostulanteId");

            migrationBuilder.CreateIndex(
                name: "IX_Documentos_TipoDocumentoId",
                table: "Documentos",
                column: "TipoDocumentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Estudios_NivelEstudiosId",
                table: "Estudios",
                column: "NivelEstudiosId");

            migrationBuilder.CreateIndex(
                name: "IX_Estudios_PostulanteId",
                table: "Estudios",
                column: "PostulanteId");

            migrationBuilder.CreateIndex(
                name: "IX_Familiares_ParentescoId",
                table: "Familiares",
                column: "ParentescoId");

            migrationBuilder.CreateIndex(
                name: "IX_Familiares_PostulanteId",
                table: "Familiares",
                column: "PostulanteId");

            migrationBuilder.CreateIndex(
                name: "IX_Seguimientos_EstadoId",
                table: "Seguimientos",
                column: "EstadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Seguimientos_PostulanteId",
                table: "Seguimientos",
                column: "PostulanteId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Seguimientos_TipoInscripcionId",
                table: "Seguimientos",
                column: "TipoInscripcionId");

            migrationBuilder.CreateIndex(
                name: "IX_Trabajo_PostulanteId",
                table: "Trabajo",
                column: "PostulanteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Domicilios_Localidades_LocalidadId",
                table: "Domicilios",
                column: "LocalidadId",
                principalTable: "Localidades",
                principalColumn: "LocalidadId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Postulantes_Nacionalidades_NacionalidadId",
                table: "Postulantes",
                column: "NacionalidadId",
                principalTable: "Nacionalidades",
                principalColumn: "NacionalidadId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Postulantes_Sexos_SexoId",
                table: "Postulantes",
                column: "SexoId",
                principalTable: "Sexos",
                principalColumn: "SexoId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Postulantes_establecimientos_EstabSolicitudId",
                table: "Postulantes",
                column: "EstabSolicitudId",
                principalTable: "establecimientos",
                principalColumn: "EstablecimientoId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Domicilios_Localidades_LocalidadId",
                table: "Domicilios");

            migrationBuilder.DropForeignKey(
                name: "FK_Postulantes_Nacionalidades_NacionalidadId",
                table: "Postulantes");

            migrationBuilder.DropForeignKey(
                name: "FK_Postulantes_Sexos_SexoId",
                table: "Postulantes");

            migrationBuilder.DropForeignKey(
                name: "FK_Postulantes_establecimientos_EstabSolicitudId",
                table: "Postulantes");

            migrationBuilder.DropTable(
                name: "Contactos");

            migrationBuilder.DropTable(
                name: "DatosPersonales");

            migrationBuilder.DropTable(
                name: "Documentos");

            migrationBuilder.DropTable(
                name: "establecimientos");

            migrationBuilder.DropTable(
                name: "Estudios");

            migrationBuilder.DropTable(
                name: "Familiares");

            migrationBuilder.DropTable(
                name: "Localidades");

            migrationBuilder.DropTable(
                name: "Seguimientos");

            migrationBuilder.DropTable(
                name: "Sexos");

            migrationBuilder.DropTable(
                name: "Trabajo");

            migrationBuilder.DropTable(
                name: "EstadosCiviles");

            migrationBuilder.DropTable(
                name: "Generos");

            migrationBuilder.DropTable(
                name: "TipoDocumentos");

            migrationBuilder.DropTable(
                name: "NivelEstudios");

            migrationBuilder.DropTable(
                name: "Parentescos");

            migrationBuilder.DropTable(
                name: "Estados");

            migrationBuilder.DropTable(
                name: "TipoInscripcions");

            migrationBuilder.DropIndex(
                name: "IX_Postulantes_Apellido_Nombre",
                table: "Postulantes");

            migrationBuilder.DropIndex(
                name: "IX_Postulantes_EstabSolicitudId",
                table: "Postulantes");

            migrationBuilder.DropIndex(
                name: "IX_Postulantes_Mail",
                table: "Postulantes");

            migrationBuilder.DropIndex(
                name: "IX_Postulantes_NacionalidadId",
                table: "Postulantes");

            migrationBuilder.DropIndex(
                name: "IX_Domicilios_LocalidadId",
                table: "Domicilios");

            migrationBuilder.DropColumn(
                name: "EstabSolicitudId",
                table: "Postulantes");

            migrationBuilder.DropColumn(
                name: "FechaSoclicitud",
                table: "Postulantes");

            migrationBuilder.DropColumn(
                name: "Mail",
                table: "Postulantes");

            migrationBuilder.DropColumn(
                name: "NacionalidadId",
                table: "Postulantes");

            migrationBuilder.RenameColumn(
                name: "SexoId",
                table: "Postulantes",
                newName: "IdNacionalidad");

            migrationBuilder.RenameIndex(
                name: "IX_Postulantes_SexoId",
                table: "Postulantes",
                newName: "IX_Postulantes_IdNacionalidad");

            migrationBuilder.RenameColumn(
                name: "NacionalidadId",
                table: "Nacionalidades",
                newName: "IdNacionalidad");

            migrationBuilder.RenameColumn(
                name: "LocalidadId",
                table: "Domicilios",
                newName: "IdLocalidad");

            migrationBuilder.AddForeignKey(
                name: "FK_Postulantes_Nacionalidades_IdNacionalidad",
                table: "Postulantes",
                column: "IdNacionalidad",
                principalTable: "Nacionalidades",
                principalColumn: "IdNacionalidad",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
