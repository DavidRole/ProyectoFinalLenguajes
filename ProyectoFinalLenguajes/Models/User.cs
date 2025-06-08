using System.ComponentModel.DataAnnotations;

namespace ProyectoFinalLenguajes.Models
{
    public class User
    {
        public User() { }

        [Key]
        public string Email { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
