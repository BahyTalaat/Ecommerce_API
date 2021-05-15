using Ecommerce.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Ecommerce.Controllers
{
    [Authorize]
    public class FaviorateController : ApiController
    {
        private ApplicationDBContext db = new ApplicationDBContext();




        //[ResponseType(typeof(ProductCartDto))]
        [HttpGet]
        public IHttpActionResult Get_Faviorate()
        {

            var user_id = User.Identity.GetUserId();
            var product_FaviorateCart = db.FaviorateProducts.Where(fp => fp.User_Id == user_id).ToList();
            var url = HttpContext.Current.Request.Url;


            ProductCartDto proCart = new ProductCartDto();
            List<ProductDto> prodtoList = new List<ProductDto>();

            foreach (var item in product_FaviorateCart)
            {
                var pro = db.Products.Find(item.Product_Id);
                ProductDto prodto = new ProductDto();
                prodto.Image = url.Scheme + "://" + url.Host + ":" + url.Port + "/Image/" + pro.Image;
                prodto.Name = pro.Name;
                prodto.Price = pro.Price;
                prodto.Id = pro.Id;
                //prodto.Quentity = item.Quntity;
                prodto.Discount = pro.Discount;
                prodto.Description = pro.Description;
                prodtoList.Add(prodto);

            }
            proCart.Productss = prodtoList;

            return Ok(proCart);
        }

        ////////////////////
        [HttpPost]
        public IHttpActionResult Post_Pro(int Product_id)
        {
            var user_ID = User.Identity.GetUserId();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var productInFavirateCart = db.FaviorateProducts.Where(p => p.User_Id == user_ID && p.Product_Id == Product_id).FirstOrDefault();
            if (productInFavirateCart != null)
            {
                return Ok("Product already Exit in Faviorate cart");
            }
   
            FaviorateProduct NewFavorateProduct = new FaviorateProduct { User_Id = user_ID, Product_Id = Product_id };

            db.FaviorateProducts.Add(NewFavorateProduct);
            db.SaveChanges();

            return Ok("Product Add To Faviorate cart");
        }
        public IHttpActionResult DeleteProduct(int id)
        {
            var user_id = User.Identity.GetUserId();
            var productInFavirateCart = db.FaviorateProducts.Where(pc => pc.User_Id == user_id).ToList();

            foreach (var item in productInFavirateCart)
            {
                if (item.Product_Id == id)
                {
                    FaviorateProduct FavPro = db.FaviorateProducts.Find(item.Id);
                    if (FavPro == null)
                    {
                        return NotFound();
                    }
                    db.FaviorateProducts.Remove(FavPro);
                    db.SaveChanges();
                }
            }
            return Ok("Product Deleted From Faviorate Cart");
        }
    }
}
