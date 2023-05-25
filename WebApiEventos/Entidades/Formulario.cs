using System.ComponentModel.DataAnnotations;

namespace WebApiEventos.Entidades
{
    public class Formulario
    {
        
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Mensaje { get; set; }
    }
}
