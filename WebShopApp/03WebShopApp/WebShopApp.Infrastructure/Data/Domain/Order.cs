using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore.Query.Internal;

namespace WebShopApp.Infrastructure.Data.Domain
{
    public class Order
    {
        public int Id { get; set; }
        [Required]
        public DateTime OrderTime { get; set; }

        [Required]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        [Required]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; } = null!;


        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalPrice { get { return this.Quantity * this.Price - this.Quantity * this.Price * this.Discount / 100; } }
       
    }
}
