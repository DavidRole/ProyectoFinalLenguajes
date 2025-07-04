using Microsoft.AspNetCore.Identity;

namespace ProyectoFinalLenguajes.Models
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsAble {  get; set; }

        public string Address { get; set; }
    }
}
