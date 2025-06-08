using System.ComponentModel.DataAnnotations;

namespace ProyectoFinalLenguajes.Models
{
    public class Customer: User
    {
        public Customer(): base() { }

        [Required]
        public string Address { get; set; }
    }
}
