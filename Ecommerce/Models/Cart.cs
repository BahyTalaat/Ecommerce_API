using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ecommerce.Models
{
    public class Cart
    {
        [ForeignKey("User")]
        [Key]
        public string User_Id { get; set; }
        [JsonIgnore]
        public virtual IdentityUser User { get; set; }
        [JsonIgnore]
        public virtual List<ProductCart> ProductsCart { get; set; }
    }
}