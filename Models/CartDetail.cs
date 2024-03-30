using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models
{
    [Table("CartDetail")]
    public class CartDetail
    {
        public int Id { get; set; }
        public int ShoppingCartId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int UnitPriced { get; set; }
        public Product Product { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
    }
}
