using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace StoreSupplier.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        [Required]
        public string CardNumber { get; set; }
        [Required]
        public string CardValidDate { get; set; }
        [Required]
        public string CVV { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; } 
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string UserId { get; set; }
        [ForeignKey("MostSoldProductId")]
        public virtual MostSoldProduct MostSoldProduct { get; set; }
        public string MostSoldProductId { get; set; } = null;
    }
}
