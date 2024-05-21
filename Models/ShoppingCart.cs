﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Innerglow_App.Models
{
    [Table("ShoppingCart")]
    public class ShoppingCart
    {
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public bool isDeleted { get; set; } = false;

        public ICollection<CartDetail> CartDetails { get; set; }
    }
}