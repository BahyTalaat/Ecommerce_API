using Ecommerce.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace Ecommerce.Controllers
{
    
    public class AccountsController : ApiController
    {
        [HttpGet]
        [Authorize]
        [Route("api/Accounts")]
        public IHttpActionResult Get()
        {
            var user_id = User.Identity.GetUserId();
            ApplicationUserManager manager = new ApplicationUserManager(new ApplicationDBContext());
            ApplicationIdentityUser user = manager.FindById(user_id);

            var url = HttpContext.Current.Request.Url;

            UserModel userDto = new UserModel();
            userDto.Name=user.UserName;
            userDto.Password=user.PasswordHash;
            userDto.Image= url.Scheme +"://" + url.Host + ":" + url.Port + "/Image/" + user.Image;
            userDto.Gender=user.Gender;
            userDto.Email=user.Email;


            if (user == null)
                return NotFound();
            return Ok(userDto);
        }

        [Authorize]
        [Route("api/Accounts")]
        public IHttpActionResult Put(UserModel model)
        {
            var user_id = User.Identity.GetUserId();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ApplicationUserManager manager = new ApplicationUserManager(new ApplicationDBContext());


            ApplicationIdentityUser user = manager.FindById(user_id);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                user.UserName = model.Name;
                user.PasswordHash = manager.PasswordHasher.HashPassword(model.Password);
                user.Gender = model.Gender;
                user.Image = model.Image;
                user.Email = model.Email;
                IdentityResult result = manager.Update(user);
                if (result.Succeeded)
                {
                    return Ok(user);
                }
                else
                {
                    return BadRequest(result.Errors.FirstOrDefault());
                }
            }
        }



        [HttpPost]
        [ResponseType(typeof(UserModel))]

        [Route("api/Accounts")]
        public async Task<IHttpActionResult> Post(UserModel account)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                ApplicationUserManager manager = new ApplicationUserManager(new ApplicationDBContext());
                ApplicationIdentityUser user = new ApplicationIdentityUser();

                user.UserName = account.Name;
                user.PasswordHash = account.Password;
                user.Email = account.Email;
                //user.BirthDate = account.BirthDate;
                user.Image = account.Image;
                user.Gender = account.Gender;

                IdentityResult result =await manager.CreateAsync(user,account.Password);
                if (result.Succeeded)
                {
                    return Created("", "register Sucess " + user.UserName);
                }
                else
                    return BadRequest((result.Errors.ToList())[0]);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
