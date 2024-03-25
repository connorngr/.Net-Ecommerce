using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;
using WebApp.Areas.Identity.Data;

namespace WebApp.Models
{
    [Table("Order")]
    public class Order
        
    {
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        [Required]
        public int OrderStatusId { get; set; }
        public bool IsDeleted { get; set; } = false;
        /*public BigInteger TotalAmount {  get; set; }*/
        public OrderStatus OrderStatus { get; set; }
        public List<OrderDetail> OrderDetail { get; set; }
        public User User { get; set; }

    }
}
