using System.Text.Json.Serialization;

namespace WebApiEventos.Entidades
{
    public class Asistencia
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        
        [JsonIgnore]
        public Usuario User { get; set; }
        public int EventId { get; set; }
        public Evento Event { get; set; }
        public DateTime RegistrationDate { get; set; }
        public bool HasAttended { get; set; }
    }
}
