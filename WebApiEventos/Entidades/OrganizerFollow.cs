namespace WebApiEventos.Entidades
{
    public class OrganizerFollow
    {
        public int UserId { get; set; }
        public Usuario User { get; set; }
        public int OrganizerId { get; set; }
        public Organizer Organizer { get; set; }
    }
}
