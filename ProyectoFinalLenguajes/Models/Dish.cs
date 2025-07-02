using System.ComponentModel.DataAnnotations;

namespace ProyectoFinalLenguajes.Models
{
    public class Dish
    {
        public Dish() { }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public double Price { get; set; }

        
        public string? ImageUrl { get; set; }

        [Required]
        public bool isAble { get; set; }
    }
}
