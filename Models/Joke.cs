using Microsoft.AspNetCore.Identity;

namespace JokesApp.Models
{
    public class Joke
    {
        public int Id { get; set; }
        public string JokesQuestion { get; set; }
        public string JokesAnswer { get; set; }
        public string? UserId { get; set; }

        public IdentityUser? User { get; set; }
        public Joke() { }
    }
}
