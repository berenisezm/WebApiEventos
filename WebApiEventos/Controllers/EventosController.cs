using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiEventos.Entidades;



namespace WebApiEventos.Controllers
{
    [ApiController]
    [Route("eventos")]
    public class EventosController : ControllerBase
    {
        private readonly EventoDbContext dbcontext;
        private object _smtpClient;

        public EventosController(EventoDbContext context)
        {
            this.dbcontext = context;
        }
                
        //Crear evento
        [HttpPost("crear")]
        public async Task<ActionResult<Evento>> CreateEvent(Evento @event)
        {
            dbcontext.Eventos.Add(@event);
            await dbcontext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEvent), new { id = @event.Id }, @event);
        }

        //Lista de eventos creados
        [HttpGet("lista")]
        public async Task<ActionResult<IEnumerable<Evento>>> GetEvents()
        {
            return await dbcontext.Eventos.ToListAsync();
        }

        //Buscar evento por id
        [HttpGet("buscar/{id}")]
        public async Task<ActionResult<Evento>> GetEvent(int id)
        {
            var @event = await dbcontext.Eventos.FindAsync(id);

            if (@event == null)
            {
                return NotFound();
            }

            return @event;
        }

        //Buscar evento por nombre
        [HttpGet("buscar/nombre")]
        public async Task<ActionResult<IEnumerable<Evento>>> SearchEventsByName(string name)
        {
            var events = await dbcontext.Eventos
                .Where(e => e.Nombre.Contains(name))
                .ToListAsync();

            return events;
        }

        //Buscar evento por fecha
        [HttpGet("buscar/fecha")]
        public async Task<ActionResult<IEnumerable<Evento>>> SearchEventsByDate(DateTime date)
        {
            var events = await dbcontext.Eventos
                .Where(e => e.FechaHora.Date == date.Date)
                .ToListAsync();

            return events;

        }

        //Buscar evento por ubicación
        [HttpGet("buscar/ubicacion")]
        public async Task<ActionResult<IEnumerable<Evento>>> SearchEventsByLocation(string location)
        {
            var events = await dbcontext.Eventos
                .Where(e => e.Ubicacion.Contains(location))
                .ToListAsync();

            return events;
        }

        //Registrar a un evento
        [HttpPost("{id}/registro")]
        public async Task<ActionResult<Evento>> RegisterForEvent(int id, int userId)
        {
            var @event = await dbcontext.Eventos.Include(e => e.Asistencias).FirstOrDefaultAsync(e => e.Id == id);
            var user = await dbcontext.Usuarios.FindAsync(userId);

            if (@event == null || user == null)
            {
                return NotFound();
            }

            if (@event.CapacidadMaxima <= 0)
            {
                return BadRequest("No hay lugares disponibles para este evento.");
            }

            var attendance = new Asistencia
            {
                Event = @event,
                User = user,
                RegistrationDate = DateTime.Now,
                HasAttended = false
            };

            @event.Asistencias.Add(attendance);
            @event.CapacidadMaxima--;

            await dbcontext.SaveChangesAsync();

            return @event;
        }

        //Cambia la asistencia
        [HttpPut("{id}/asistencia")]
        public async Task<IActionResult> MarkAttendance(int id, int userId)
        {
            var attendance = await dbcontext.Asistencias
                .FirstOrDefaultAsync(a => a.EventId == id && a.UsuarioId == userId);

            if (attendance == null)
            {
                return NotFound();
            }

            attendance.HasAttended = true;

            await dbcontext.SaveChangesAsync();

            return NoContent();
        }

        //Eventos más populares
        [HttpGet("populares")]
        public async Task<ActionResult<IEnumerable<Evento>>> GetPopularEvents()
        {
            var events = await dbcontext.Eventos
                .OrderByDescending(e => e.Asistencias.Count)
                .ToListAsync();

            return events;
        }

        //El administrador pueda editar el evento        
        [HttpPut("{eventId}/editar")]
        public async Task<IActionResult> UpdateEvent(int eventId, Evento updatedEvent)
        {
            var eventToUpdate = await dbcontext.Eventos.FindAsync(eventId);

            if (eventToUpdate == null)
            {
                return NotFound();
            }

            var organizerId = eventToUpdate.AdminId;

            if (eventToUpdate.AdminId != organizerId)
            {
                return Forbid(); 
            }

            eventToUpdate.Nombre = updatedEvent.Nombre;
            eventToUpdate.Descripcion = updatedEvent.Descripcion;
            eventToUpdate.FechaHora = updatedEvent.FechaHora;
            eventToUpdate.Ubicacion = updatedEvent.Ubicacion;
            eventToUpdate.CapacidadMaxima = updatedEvent.CapacidadMaxima;


            await dbcontext.SaveChangesAsync();

            return NoContent();
        }

        //Permitir seguir a un organizador para recibir actualizaciones            
        [HttpPost("{organizerId}/seguir")]
        public async Task<IActionResult> FollowOrganizer(int organizerId)
        {
            // Obtener el organizador
            var organizer = await dbcontext.Organizers.FindAsync(organizerId);
            if (organizer == null)
            {
                return NotFound();
            }

            // Obtener el usuario actual (puedes implementar tu propio sistema de autenticación/autorización)
            var userId = 1; // Ejemplo: ID de usuario fijo para simplificar
            var user = await dbcontext.Usuarios.FindAsync(userId);

            // Verificar si el usuario ya sigue al organizador
            var existingFollow = await dbcontext.OrganizerFollows.FindAsync(userId, organizerId);
            if (existingFollow != null)
            {
                return BadRequest("Ya sigues a este organizador.");
            }

            // Crear la relación de seguimiento
            var follow = new OrganizerFollow
            {
                UserId = userId,
                OrganizerId = organizerId
            };

            dbcontext.OrganizerFollows.Add(follow);
            await dbcontext.SaveChangesAsync();

            return Ok("Ahora sigues a este organizador.");
        }

        //Permitir a los usuarios marcar eventos como favoritos
        /*public FavoriteEventsController(EventoDbContext dbContext)
        {
            EventoDbContext = dbContext;
        }

        // Obtener eventos favoritos de un usuario específico
        [HttpGet("{userId}")]
        public IActionResult GetFavoriteEvents(string userId)
        {
            var favoriteEvents = EventoDbContext.FavoriteEvents
                .Where(fe => fe.UserId == userId)
                .Select(fe => fe.Event)
                .ToList();

            return Ok(favoriteEvents);
        }

        // Marcar un evento como favorito para un usuario específico
        [HttpPost]
        public IActionResult MarkAsFavorite(FavoriteEvent favoriteEvent, EventoDbContext EventoDbContext)
        {
            object favoritos = EventoDbContext.FavoriteEvents.Add(favoriteEvent);
            EventoDbContext.SaveChanges();

            return Ok();
        }

        public EventoDbContext GetEventoDbContext()
        {
            return EventoDbContext;
        }

        // Desmarcar un evento como favorito para un usuario específico
        [HttpDelete("{favoriteEventId}")]
        public IActionResult RemoveFavorite(int favoriteEventId, EventoDbContext eventoDbContext)
        {
            var favoriteEvent = EventoDbContext.FavoriteEvents.Find(favoriteEventId);

            if (favoriteEvent == null)
            {
                return NotFound();
            }

            EventoDbContext.FavoriteEvents.Remove(favoriteEvent);
            int cambios = eventoDbContext.SaveChanges();

            return Ok();
        }

        //Permitir a los organizadores crear descuentos y promociones y enviar la promo a los usuarios registrados

        private readonly PromotionRepository _repository;
        private readonly object _smtpClient;

        public PromotionController(PromotionRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{id}")]
        public ActionResult<Promotion> GetPromotionById(int id)
        {
            var promotion = _repository.GetPromotionById(id);
            if (promotion == null)
            {
                return NotFound();
            }

            return promotion;
        }

        [HttpPost]
        public ActionResult CreatePromotion(Promotion promotion)
        {
            _repository.AddPromotion(promotion);
            return CreatedAtAction(nameof(GetPromotionById), new { id = promotion.Id }, promotion);
        }

        [HttpPut("{id}")]
        public IActionResult UpdatePromotion(int id, Promotion promotion)
        {
            if (id != promotion.Id)
            {
                return BadRequest();
            }

            _repository.UpdatePromotion(promotion);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePromotion(int id)
        {
            _repository.DeletePromotion(id);
            return NoContent();
        }

        //Enviar recordatorios
        [HttpPost("recordatorios/{eventoId}")]
        public IActionResult EnviarRecordatorios(int eventoId)
        {
            try
            {
                // Obtener la lista de usuarios registrados al evento desde tu repositorio de datos
                var dbContext = new EventoDbContext(); // Tu clase de contexto de base de datos
                var usuariosRegistrados = dbContext.Usuarios.Where(u => u.Event == eventoId).ToList();

                // Calcular la fecha para enviar los recordatorios
                var fechaRecordatorio = DateTime.Now.AddDays(1);

                // Crear y enviar los correos de recordatorio
                foreach (var usuario in usuariosRegistrados)
                {
                    var correo = new MailMessage
                    {
                        From = new MailAddress("tu_correo@example.com"),
                        Subject = "Recordatorio de evento",
                        Body = $"Estimado {usuario.Name}, recuerda que tienes un evento el {fechaRecordatorio.ToShortDateString()}."
                    };

                    correo.To.Add(usuario.Email);

                    // Envío del correo electrónico
                    //object enviarcorreo = _smtpClient.Send(correo);
                }

                return Ok("Los recordatorios han sido enviados correctamente.");
            }
            catch (Exception ex)
            {
                // Manejo de errores
                return StatusCode(500, $"Ha ocurrido un error al enviar los recordatorios: {ex.Message}");
            }
        }
        */
    }
}