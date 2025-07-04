using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices.Marshalling;
using System.Text.Json.Serialization;

namespace ProyectoFinalLenguajes.Models
{
    public class OrderDetail
    {
        public OrderDetail()
        {
            
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        public int DishId { get; set; }

        [JsonIgnore]
        [ForeignKey("OrderId")]
        public virtual Order Order { get ; set; }

        [ForeignKey("DishId")]
        public virtual Dish Dish { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}
