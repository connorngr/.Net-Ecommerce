using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Innerglow_App.Models
{
    [Table("OrderStatus")]
    public class OrderStatus
    {
        public int Id { get; set; }
        [Required]
        public int StatusId { get; set; }
        [Required]
        public string? StatusName { get; set; }
    }
}
