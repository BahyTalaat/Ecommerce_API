using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using Ecommerce.Models;

namespace Ecommerce.Controllers
{
    public class CategoriesController : ApiController
    {
        private ApplicationDBContext db = new ApplicationDBContext();

        // GET: api/Categories
        public IQueryable<Category> GetCategories()
        {
            var url = HttpContext.Current.Request.Url;
            foreach (var category in db.Categories)
            {
                category.Image = url.Scheme + "://" + url.Host + ":" + url.Port + "/Image/" + category.Image;
            }

            return db.Categories;
        }

        
        [Route("api/ProductsCategory/{Cat_id}")]
        public IEnumerable<Product> GetCategoryProducts(int Cat_id)
        {

            var Products = db.Products.Where(p=>p.Category_Id== Cat_id).ToList();
            foreach (var product in Products)
            {
                var url = HttpContext.Current.Request.Url;
                product.Image = url.Scheme + "://" + url.Host + ":" + url.Port + "/Image/" + product.Image;
            }
            return Products;
            
        }

        // GET: api/Categories/5
        [ResponseType(typeof(CategoryDto))]
        public IHttpActionResult GetCategory(int id)
        {
            var url = HttpContext.Current.Request.Url;
            Category category = db.Categories.Find(id);

            CategoryDto catDto = new CategoryDto();

            catDto.Id = category.Id;
            catDto.Name = category.Name;
            catDto.Image = url.Scheme +"://"+url.Host+":"+url.Port+"/Image/"+ category.Image;
            List<ProductDto> productslist = new List<ProductDto>();

            for(var i=0;i< category.Products.Count;i++)
            {
                ProductDto prodDto = new ProductDto();
                
                prodDto.Image = url.Scheme + "://" + url.Host + ":" + url.Port + "/Image/" + category.Products[i].Image;
                prodDto.Name = category.Products[i].Name;
                prodDto.Price = category.Products[i].Price;
                prodDto.Quentity = category.Products[i].Quentity;
                prodDto.Discount = category.Products[i].Discount;
                prodDto.Id = category.Products[i].Id;
                productslist.Add(prodDto);
               
            }
            catDto.Products = productslist;


            if (category == null)
            {
                return NotFound();
            }

            return Ok(catDto);
        }

        // PUT: api/Categories/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCategory(int id, Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != category.Id)
            {
                return BadRequest();
            }

            db.Entry(category).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Categories
        [ResponseType(typeof(Category))]
        public IHttpActionResult PostCategory(Category category)
        {
            if (!ModelState.IsValid)
            {  
                return BadRequest(ModelState);
            }

            db.Categories.Add(category);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = category.Id }, category);
        }

        // DELETE: api/Categories/5
        [HttpDelete]
        [ResponseType(typeof(Category))]
        public IHttpActionResult DeleteCategory(int id)
        {
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            db.Categories.Remove(category);
            db.SaveChanges();

            return Ok(category);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CategoryExists(int id)
        {
            return db.Categories.Count(e => e.Id == id) > 0;
        }
    }
}