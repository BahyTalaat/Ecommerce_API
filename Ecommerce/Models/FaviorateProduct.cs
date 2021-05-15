using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ecommerce.Models
{
    public class FaviorateProduct
    {
        public int Id { get; set; }

        [ForeignKey("User")]
        public string User_Id { get; set; }
        public virtual ApplicationIdentityUser User { get; set; }


        [ForeignKey("Product")]
        public int Product_Id { get; set; }
        public virtual Product Product { set; get; }
    }
}