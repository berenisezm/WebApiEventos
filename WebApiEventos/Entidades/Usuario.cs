
using System.Text.Json.Serialization;

namespace WebApiEventos.Entidades
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        [JsonIgnore]
        public List<Asistencia> Asistencias { get; set; }
        public ICollection<OrganizerFollow> OrganizerFollows { get; set; }
        
        
    }

}