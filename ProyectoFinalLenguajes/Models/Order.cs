using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinalLenguajes.Models
{
    public class Order
    {
        public Order()  
        {
            OrderDishes = new HashSet<OrderDetail>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string CustomerId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime Date { get; set; }

       
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }

        public virtual ICollection<OrderDetail> OrderDishes { get; set; }

    }
}
