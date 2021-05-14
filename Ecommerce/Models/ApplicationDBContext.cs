using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Ecommerce.Models
{
    public class ApplicationIdentityUser : IdentityUser
    {
        public Gender Gender { get; set; }

        //[DataType(DataType.Date)]
        //public DateTime BirthDate { get; set; }

        public string Image { get; set; }

    }
    public enum Gender
    {
        Male, Female
    }
    public class ApplicationUserStore : UserStore<ApplicationIdentityUser>
    {
        public ApplicationUserStore(DbContext db) : base(db) { }
    }
    public class ApplicationRoleManager : RoleManager<IdentityRole>
    {
        public ApplicationRoleManager(DbContext db)
            : base(new RoleStore<IdentityRole>(db))
        {

        }
    }
    public class ApplicationUserManager : UserManager<ApplicationIdentityUser>
    {
        public ApplicationUserManager(DbContext db) : base(new ApplicationUserStore(db)) { }
    }

    public class ApplicationDBContext:IdentityDbContext<ApplicationIdentityUser>
    {
        public ApplicationDBContext() : base("CS")
        {

        }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<ProductCart> ProductCarts { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<ProductOrder> ProductOrders { get; set; }
    }
}