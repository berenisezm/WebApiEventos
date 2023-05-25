using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiEventos.Entidades;


namespace WebApiEventos.Controllers
{
    [ApiController]
    [Route("usuarios")]
    public class UsuariosController : ControllerBase
    {
        private readonly EventoDbContext dbcontext;

        public UsuariosController(EventoDbContext context)
        {
            this.dbcontext = context;
        }

        [HttpPost]
        public async Task<ActionResult<Usuario>> CreateUser(Usuario user)
        {
            dbcontext.Usuarios.Add(user);
            await dbcontext.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUser()
        {
            return await dbcontext.Usuarios.ToListAsync();
        }

        //Historial de eventos de un usuario
        [HttpGet("{userId}/historial")]
        public async Task<ActionResult<IEnumerable<Asistencia>>> GetUserAttendance(int userId)
        {
            var user = await dbcontext.Usuarios.Include(u => u.Asistencias)
                                           .ThenInclude(a => a.Event)
                                           .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return NotFound();
            }

            var historialAsistencia = user.Asistencias;

            return Ok(historialAsistencia);
        }
    }
}
