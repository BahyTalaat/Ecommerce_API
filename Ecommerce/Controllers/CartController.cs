using Ecommerce.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace Ecommerce.Controllers
{
    [Authorize]
    public class CartController : ApiController
    {
        private ApplicationDBContext db = new ApplicationDBContext();


        

        [ResponseType(typeof(ProductCartDto))]
        [HttpGet]
        public IHttpActionResult Get_Cart()
        {

            var user_id = User.Identity.GetUserId();
            var product_Cart = db.ProductCarts.Where(pc => pc.Cart_Id == user_id).ToList();
            var url = HttpContext.Current.Request.Url;

            ProductCartDto proCart = new ProductCartDto();
            List<ProductDto> prodtoList = new List<ProductDto>();
            foreach(var item in product_Cart)
            {
                var pro=db.Products.Find(item.Product_Id);
                ProductDto prodto = new ProductDto();
                prodto.Image = url.Scheme + "://" + url.Host + ":" + url.Port + "/Image/" + pro.Image;
                prodto.Name = pro.Name;
                prodto.Price = pro.Price;
                prodto.Id = pro.Id;
                //prodto.Quentity = pro.Quentity;
                prodto.Quentity=item.Quntity;
                prodto.Discount = pro.Discount;
                prodto.Description = pro.Description;
                prodtoList.Add(prodto);

            }
            proCart.Productss = prodtoList;
           
            return Ok(proCart);
        }

        // http://localhost:13149/api/Cart?Product_id=1

        [HttpPost]
        public IHttpActionResult Post_Pro(int Product_id,int qty)
        {
            var user_ID = User.Identity.GetUserId();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var Checkcart = db.Carts.Find(user_ID);
            if (Checkcart == null)
            {
                Cart cart = new Cart { User_Id = user_ID };
                db.Carts.Add(cart);
                db.SaveChanges();
            }

            var productInCart = db.ProductCarts.Where(p => p.Cart_Id == user_ID && p.Product_Id == Product_id).FirstOrDefault();
            if (productInCart != null)
            {
                productInCart.Quntity += qty;
                db.Entry(productInCart).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                    return Ok("Product Add To cart");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(Product_id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }



            }
            ProductCart pro_cart = new ProductCart { Cart_Id = user_ID, Product_Id = Product_id, Quntity = qty };

            db.ProductCarts.Add(pro_cart);
            db.SaveChanges();



            //return CreatedAtRoute("DefaultApi", new { id = pro_cart.Id }, pro_cart);
            return Ok("Product Add To cart");
        }


        [HttpPut]
        //[Route("api/cart/{productId}/{qty}")]
        public IHttpActionResult Put(int id,ProductDto product)
        {
            var user_id = User.Identity.GetUserId();
            ProductCart productCart = db.ProductCarts.Where(p => p.Cart_Id == user_id && p.Product_Id == id).FirstOrDefault();

            productCart.Quntity = product.Quentity;

            db.Entry(productCart).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
                return Ok("Product edited in cart");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Delete Productfrom Cart
        public IHttpActionResult DeleteProduct(int id)
        {
            var user_id = User.Identity.GetUserId();
            var product_Cart = db.ProductCarts.Where(pc => pc.Cart_Id == user_id).ToList();

            foreach (var item in product_Cart)
            {
                if (item.Product_Id == id)
                {
                    ProductCart proCart = db.ProductCarts.Find(item.Id);
                    if (proCart == null)
                    {
                        return NotFound();
                    }
                    db.ProductCarts.Remove(proCart);
                    db.SaveChanges();
                }
            }
                return Ok("Product Deleted");
        }
        private bool CategoryExists(object id)
        {
            throw new NotImplementedException();
        }

    }
}
