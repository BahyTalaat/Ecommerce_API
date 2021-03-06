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
using System.Web.Http.Description;
using Ecommerce.Models;

namespace Ecommerce.Controllers
{
    public class ProductsController : ApiController
    {
        private ApplicationDBContext db = new ApplicationDBContext();

        // GET: api/Products
        public IQueryable<Product> GetProducts()
        {
            var Products  = db.Products;
            var url = HttpContext.Current.Request.Url;
            foreach (var product in Products)
            {
                product.Image = url.Scheme + "://" + url.Host + ":" + url.Port + "/Image/" + product.Image;
            }
            return Products;
        }

        [Route("api/search/{productName}")]
        public IQueryable<Product> GetByName(string productName)
        {
            var url = HttpContext.Current.Request.Url;
            var products = db.Products.Where(e => e.Name.Contains(productName));
            foreach (var pro in products)
            {
                pro.Image = url.Scheme + "://" + url.Host + ":" + url.Port + "/Image/" + pro.Image;
            }
            return products;
        }


        // GET: api/Products/5
        [ResponseType(typeof(Product))]
        public IHttpActionResult GetProduct(int id)
        {

            Product product = db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            //string path = System.Web.HttpContext.Current.Request.MapPath("~\\image\\"+product.Image);
            
            var url = HttpContext.Current.Request.Url;
            product.Image = url.Scheme + "://" + url.Host + ":" + url.Port + "/Image/" + product.Image;
            //product.Image = "/Image/" + product.Image;
            //product.Image = path;
            return Ok(product);
        }

        // PUT: api/Products/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProduct(int id, Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.Id)
            {
                return BadRequest();
            }

            db.Entry(product).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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

        // POST: api/Products
        [ResponseType(typeof(Product))]
        public IHttpActionResult PostProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Products.Add(product);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = product.Id }, product);
        }

        // DELETE: api/Products/5
        [ResponseType(typeof(Product))]
        public IHttpActionResult DeleteProduct(int id)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            db.Products.Remove(product);
            db.SaveChanges();

            return Ok(product);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductExists(int id)
        {
            return db.Products.Count(e => e.Id == id) > 0;
        }
    }
}