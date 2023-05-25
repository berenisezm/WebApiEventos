using Microsoft.AspNetCore.Mvc;
using WebApiEventos.Entidades;

namespace WebApiEventos.Controllers
{
    /*[ApiController]
    [Route("api/[controller]")]
    public class ComentariosController : ControllerBase
    {
        private readonly EventoDbContext dbContext;

        public ComentariosController(EventoDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public EventoDbContext GetDbContext()
        {
            return dbContext;
        }

        [HttpPost]
        public IActionResult EnviarComentario([FromBody] Comentario comentario, EventoDbContext dbContext)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            dbContext.Comentarios.Add(comentario);
            dbContext.SaveChanges();

            return Ok("Comentario enviado correctamente");
        }
    }*/
}
