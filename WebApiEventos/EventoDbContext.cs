using Microsoft.EntityFrameworkCore;
using WebApiEventos.Entidades;

namespace WebApiEventos
{
    public class EventoDbContext : DbContext
    {
        internal object Comentarios;

        public EventoDbContext()
        {
        }

        public EventoDbContext(DbContextOptions<EventoDbContext> options) : base(options) { }

        
        public DbSet<Evento> Eventos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Asistencia> Asistencias { get; set; }
        public DbSet<Organizer> Organizers { get; set; }
        public DbSet<OrganizerFollow> OrganizerFollows { get; set; }
        public DbSet<Formulario> Formularios { get; set; }
        public DbSet<Promocion> Promociones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Asistencia>()
                .HasKey(a => new { a.UsuarioId, a.EventId });

            modelBuilder.Entity<Asistencia>()
                .HasOne(a => a.User)
                .WithMany(u => u.Asistencias)
                .HasForeignKey(a => a.UsuarioId);

            modelBuilder.Entity<Asistencia>()
                .HasOne(a => a.Event)
                .WithMany(e => e.Asistencias)
                .HasForeignKey(a => a.EventId); 

            modelBuilder.Entity<OrganizerFollow>()
                .HasKey(o => new { o.UserId, o.OrganizerId });

            modelBuilder.Entity<OrganizerFollow>()
                .HasOne(o => o.User)
                .WithMany(u => u.OrganizerFollows)
                .HasForeignKey(o => o.UserId);

            modelBuilder.Entity<OrganizerFollow>()
                .HasOne(o => o.Organizer)
                .WithMany(o => o.FollowerUsers)
                .HasForeignKey(o => o.OrganizerId);

            modelBuilder.Entity<Formulario>()
                .HasKey(f => f.Id);

        }
    }
}
