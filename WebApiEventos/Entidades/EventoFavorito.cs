namespace WebApiEventos.Entidades
{
    public class EventoFavorito
    {
        public string UserId { get; set; }
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaHora { get; set; }
        public string Ubicacion { get; set; }
        public int CapacidadMaxima { get; set; }
        public int AdminId { get; set; }
        public Usuario Administrador { get; set; }
        public List<Asistencia> Asistencias { get; set; }
    }
}
