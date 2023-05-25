namespace WebApiEventos.Entidades
{
    public class Organizer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public ICollection<OrganizerFollow> FollowerUsers { get; set; }

    }
}
