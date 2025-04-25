using Microsoft.EntityFrameworkCore;

namespace API_Reclutamiento.Models
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

        public DbSet<Postulante> Postulantes { get; set; }
        public DbSet<Domicilio> Domicilios { get; set; }
        public DbSet<Nacionalidad> Nacionalidades { get; set; }
        public DbSet<DatosPersonales> DatosPersonales { get; set; }
        public DbSet<Sexo> Sexos { get; set; }
        public DbSet<Genero> Generos { get; set; }  
        public DbSet<Localidad> Localidades { get; set; }
        public DbSet<EstadoCivil> EstadosCiviles { get; set; }
        public DbSet<Contacto> Contactos { get; set; }  
        public DbSet<Familiares> Familiares { get; set; }
        public DbSet<Parentesco> Parentescos { get; set; }
        public DbSet<Estudios> Estudios { get; set; }
        public DbSet<NivelEstudios> NivelEstudios { get; set; }
        public DbSet<Documento> Documentos { get; set; }    
        public DbSet<TipoDocumento> TipoDocumentos { get; set; }    
        public DbSet<Trabajo> Trabajo { get; set; } 
        public DbSet<Seguimiento> Seguimientos { get; set; }
        public DbSet<Estado> Estados { get; set; }  
        public DbSet<TipoInscripcion> TipoInscripcions { get; set; }
        public DbSet<Establecimiento> establecimientos { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Postulante>(entity =>
            {
                // Configuración de la clave primaria con autoincremento
                entity.HasKey(p => p.PostulanteId);
                entity.Property(p => p.PostulanteId)
                      .ValueGeneratedOnAdd();

                // Configuración de las propiedades del modelo
                entity.Property(p => p.Nombre)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(p => p.Apellido)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(p => p.FechaNac)
                      .IsRequired();

                entity.Property(p => p.Mail)
                      .IsRequired();

                entity.HasIndex(p => p.Mail).IsUnique();


                // Configuración de índices para búsquedas más rápidas
                entity.HasIndex(p => new { p.Apellido, p.Nombre, p.Dni });

                // Relación de muchos Postulantes con una Nacionalidad
                entity.HasOne(p => p.Nacionalidad)
                      .WithMany(n => n.Postulantes)
                      .HasForeignKey(p => p.NacionalidadId)
                      .OnDelete(DeleteBehavior.Restrict); // si al menos un postulante tiene una nacionalidad esta no se puede borrar

                // Relación de muchos Postulantes con un sexo
                entity.HasOne(p => p.Sexo)
                      .WithMany(n => n.Postulantes)
                      .HasForeignKey(p => p.SexoId)
                      .OnDelete(DeleteBehavior.Restrict); // si al menos un postulante tiene un sexo este no se puede borrar
                          
                // Relacion de muchos postulantes con un Establecimiento
                entity.HasOne(p => p.Establecimiento)
                        .WithMany(e => e.Postulantes)
                        .HasForeignKey(p => p.EstabSolicitudId)
                        .IsRequired(false)
                        .OnDelete(DeleteBehavior.Restrict);
            });


            modelBuilder.Entity<Domicilio>(entity =>
            {
                // Relación de muchos Domicilios con un postulante
                entity.HasOne(d => d.Postulante)
                        .WithMany(p => p.Domicilios)
                        .HasForeignKey(d => d.PostulanteId)
                        .OnDelete(DeleteBehavior.Cascade); // Al borrar un postulante se borran tambien sus domicilios

                // Relacion de una localidad con muchos domicilios
                entity.HasOne(d => d.Localidad)
                        .WithMany(l => l.Domicilio)
                        .HasForeignKey(d => d.LocalidadId)
                        .OnDelete(DeleteBehavior.Restrict);

            });

            modelBuilder.Entity<DatosPersonales>(entity =>
            {
                //Relacion de uno a uno con Postulante 
                entity.HasOne(d => d.Postulante)
                        .WithOne(p => p.DatosPersonales)
                        .HasForeignKey<DatosPersonales>(d => d.PostulanteId)
                        .OnDelete(DeleteBehavior.Cascade);

                //Relacion de muchos datosPersonales con un Genero
                entity.HasOne(d => d.Genero)
                        .WithMany(g => g.DatosPersonales)
                        .HasForeignKey(d => d.GeneroId)
                        .OnDelete(DeleteBehavior.Restrict);

                //Relacion de muchos datosPersonales con un EstadoCivil
                entity.HasOne(d => d.EstadoCivil)
                        .WithMany(g => g.DatosPersonales)
                        .HasForeignKey(d => d.EstadoCivilId)
                        .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Contacto>(entity =>
            {
                //relacion de un postulante a muchos contactos
                entity.HasOne(c => c.Postulante)
                        .WithMany(p => p.Contactos)
                        .HasForeignKey(c => c.PostulanteId)
                        .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Familiares>(entity =>
            {
                //relacion de un postulante a muchos familiares
                entity.HasOne(c => c.Postulante)
                        .WithMany(p => p.Familiares)
                        .HasForeignKey(c => c.PostulanteId)
                        .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(f => f.Parentesco)
                        .WithMany(p => p.Familiares)
                        .HasForeignKey(f => f.ParentescoId)
                        .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Estudios>(entity =>
            {
                //relacion de un postulante a muchos Estudios
                entity.HasOne(c => c.Postulante)
                        .WithMany(p => p.Estudios)
                        .HasForeignKey(c => c.PostulanteId)
                        .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.NivelEstudios)
                        .WithMany(n => n.Estudios)
                        .HasForeignKey(e => e.NivelEstudiosId)
                        .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Trabajo>(entity =>
            {
                entity.HasOne(t => t.Postulante)
                        .WithMany(p => p.Trabajos)
                        .HasForeignKey(t => t.PostulanteId)
                        .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Documento>(entity => 
            {
                entity.HasOne(d => d.Postulante)
                        .WithMany(p => p.Documentos)
                        .HasForeignKey(d => d.PostulanteId)
                        .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.TipoDocumento)
                        .WithMany(t => t.Documentos)
                        .HasForeignKey(d => d.TipoDocumentoId)
                        .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Seguimiento>(entity =>
            {
                entity.HasOne(s => s.Postulante)
                        .WithOne(p => p.Seguimiento)
                        .HasForeignKey<Seguimiento>(s => s.PostulanteId)
                        .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(s => s.TipoInscripcion)
                       .WithMany(t => t.Seguimientos)
                        .HasForeignKey(d => d.TipoInscripcionId)
                        .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(s => s.Estado)
                        .WithMany(e => e.Seguimientos)
                        .HasForeignKey(s => s.EstadoId)
                        .OnDelete(DeleteBehavior.Restrict);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
