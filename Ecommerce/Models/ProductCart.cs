using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ecommerce.Models
{
    public class ProductCart
    {
        public int Id { get; set; }
        [ForeignKey("Cart")]
        public string Cart_Id { get; set; }
        [JsonIgnore]
        public Cart Cart { get; set; }

        [ForeignKey("Product")]
        public int Product_Id { get; set; }
        [JsonIgnore]
        public Product Product { get; set; }

        public int Quntity { get; set; }
    }
}