using Ecommerce.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace Ecommerce.Controllers
{
    public class AccountsController : ApiController
    {
        public IHttpActionResult Get(string id)
        {
            ApplicationUserManager manager = new ApplicationUserManager(new ApplicationDBContext());
            ApplicationIdentityUser user = manager.FindById(id);

            if (user == null)
                return NotFound();
            return Ok(user);
        }

        public IHttpActionResult Put(string id, UserModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ApplicationUserManager manager = new ApplicationUserManager(new ApplicationDBContext());


            ApplicationIdentityUser user = manager.FindById(id);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                user.UserName = model.Name;
                user.PasswordHash = manager.PasswordHasher.HashPassword(model.Password);
                ////user.Gender = model.Gender;
                //user.BirthDate = model.BirthDate;
                //user.Image = model.Image;
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

        //[HttpPost]
        //public async Task<IHttpActionResult> registration(UserModel account)
        //{

        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    try
        //    {
        //        UserStore<IdentityUser> store =
        //        new UserStore<IdentityUser>(new ApplicationDBContext());

        //        UserManager<IdentityUser> manager =
        //       new UserManager<IdentityUser>(store);
        //        IdentityUser user = new IdentityUser();
        //        user.UserName = account.Name;
        //        user.PasswordHash = account.Password;
        //        user.Email = account.Email;

        //        IdentityResult result = await manager.CreateAsync(user, account.Password);
        //        if (result.Succeeded)
        //        {
        //            return Ok("ss");

        //            //return Created("", "register Sucess " + user.UserName);
        //        }
        //        else
        //            return Ok("aa");
        //        //return BadRequest((result.Errors.ToList())[0]);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok("22");
        //        //return BadRequest(ex.Message);
        //    }

        //}


        [HttpPost]
        [ResponseType(typeof(UserModel))]
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
