using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ecommerce.Models
{
    public class ProductOrder
    {
        public int Id { get; set; }

        [ForeignKey("Order")]
        public int Order_Id { get; set; }
        public virtual Order Order { set; get; }

        [ForeignKey("Product")]
        public int Product_Id { get; set; }
        public virtual Product Product { set; get; }

        public int Quntaty { get; set; }
    }
}