using Ecommerce.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace Ecommerce.Controllers
{
    [Authorize]
    public class OrderController : ApiController
    {
        private ApplicationDBContext db = new ApplicationDBContext();
       [HttpPost]
        public IHttpActionResult Post_Order()
        {
            var user_id = User.Identity.GetUserId();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Order order = new Order { User_Id = user_id, Date = DateTime.Now };
            if(order != null)
            {
                db.Orders.Add(order);
                db.SaveChanges();
            }

            
            var product_Cart = db.ProductCarts.Where(pc => pc.Cart_Id == user_id).ToList();
            //double totalPrice = 0;
            foreach (var item in product_Cart)
            {
                ProductOrder productorder = new ProductOrder();

                productorder.Order_Id = order.Id;
                productorder.Product_Id = item.Product_Id;
                productorder.Quntaty = item.Quntity;
                db.ProductOrders.Add(productorder);
                db.SaveChanges();


                

                db.ProductCarts.Remove(item);
                db.SaveChanges();

            }
           

            return Ok("Order Done");
        }

        [ResponseType(typeof(OrderDto))]
        [HttpGet]
        //[Authorize(Roles ="Admin")]
        public IHttpActionResult Get_Order()
        {
            //var user_id = User.Identity.GetUserId();

            //Order order = db.Orders.Where(o => o.User_Id == user_id).FirstOrDefault();
            ////List<Order> orders = db.Orders.Where(o => o.User_Id == user_id).ToList();

            //var product_Order = db.ProductOrders.Where(po => po.Order_Id == order.Id).ToList();


            //List<ProductDto> prodtoList = new List<ProductDto>();

            //double totalPrice = 0;
            //foreach (var item in product_Order)
            //{
            //    var pro = db.Products.Find(item.Product_Id);
            //    ProductDto prodto = new ProductDto();
            //    prodto.Image = pro.Image;
            //    prodto.Name = pro.Name;
            //    prodto.Price = pro.Price;
            //    prodto.Quentity = item.Quntaty;
            //    prodto.Discount = pro.Discount;
            //    prodto.Description = pro.Description;
            //    totalPrice += (pro.Price * item.Quntaty);
            //    prodtoList.Add(prodto);

            //    }

            //    order.TotalPrice = totalPrice;
            //    db.Entry(order).State = EntityState.Modified;
            //    db.SaveChanges();

            //OrderDto orderDto = new OrderDto { Id = order.Id, Date = order.Date, TotalPrice = order.TotalPrice,Products= prodtoList };

            //return Ok(orderDto);

            var user_id = User.Identity.GetUserId();

           
            List<Order> orders = db.Orders.Where(o => o.User_Id == user_id).ToList();

            List<OrderDto> OrdersDtos = new List<OrderDto>();
            var url = HttpContext.Current.Request.Url;
           
            foreach (var order in orders)
            {

                var product_Order = db.ProductOrders.Where(po => po.Order_Id == order.Id).ToList();

                List<ProductDto> prodtoList = new List<ProductDto>();

                double totalPrice = 0;


                foreach (var item in product_Order)
                {
                    var pro = db.Products.Find(item.Product_Id);
                    ProductDto prodto = new ProductDto();
                    prodto.Image = url.Scheme + "://" + url.Host + ":" + url.Port + "/Image/" + pro.Image;
                    prodto.Name = pro.Name;
                    prodto.Price = pro.Price;
                    prodto.Quentity = item.Quntaty;
                    prodto.Discount = pro.Discount;
                    prodto.Description = pro.Description;
                    totalPrice += (pro.Price * item.Quntaty);
                    prodtoList.Add(prodto);

                }
                order.TotalPrice = totalPrice;
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                OrderDto orderDto = new OrderDto{ Id = order.Id, Date = order.Date, TotalPrice = order.TotalPrice, Products = prodtoList };
                OrdersDtos.Add(orderDto);

            }
            return Ok(OrdersDtos);

        }
    }
}
