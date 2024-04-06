﻿using System.ComponentModel.DataAnnotations;
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
        public string UserId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public string ShippingAddress { get; set; }
        public string Notes { get; set; }
        [Required]
        public int OrderStatusId { get; set; }
        public bool IsDeleted { get; set; } = false;
        public OrderStatus OrderStatus { get; set; }
        public List<OrderDetail> OrderDetail { get; set; }
        public User User { get; set; }

    }
}
