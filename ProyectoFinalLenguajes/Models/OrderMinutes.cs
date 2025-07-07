using System.ComponentModel.DataAnnotations;

namespace ProyectoFinalLenguajes.Models
{
    public class OrderMinutes
    {
        [Key]
        public int Id { get; set; }

        [Range(1, 30)]
        public int Overtime { get; set; }
        [Range(1, 30)]
        public int Late { get; set; }


    }
}
