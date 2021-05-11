using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ecommerce.Models
{
   
    public class Category
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Image { get; set; }
        [JsonIgnore]
        public virtual List<Product> Products { get; set; }
    }
}