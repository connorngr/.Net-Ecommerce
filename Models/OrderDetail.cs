using System.ComponentModel.DataAnnotations.Schema;

namespace Innerglow_App.Models
{
    [Table("OrderDetail")]
    public class OrderDetail
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}
