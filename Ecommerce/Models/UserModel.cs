using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ecommerce.Models
{
    public class UserModel
    {

        [Required]
        public string Name { get; set; }

        //[DataType(DataType.EmailAddress)]
        [Required]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }

        [Compare("Password")]
        public string confirmPassword { get; set; }

        public Gender Gender { set; get; }

        //[Required]
        //[DataType(DataType.Date)]
        //public DateTime BirthDate { get; set; }

        [Required]
        public string Image { get; set; }
    }
}