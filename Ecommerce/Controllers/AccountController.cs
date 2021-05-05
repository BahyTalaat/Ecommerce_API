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

namespace Ecommerce.Controllers
{
    public class AccountController : ApiController
    {
        [HttpPost]
        public async Task<IHttpActionResult> registration(UserModel account)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                UserStore<IdentityUser> store =
                    new UserStore<IdentityUser>(new ApplicationDBContext());

                UserManager<IdentityUser> manager =
                    new UserManager<IdentityUser>(store);
                IdentityUser user = new IdentityUser();
                    user.UserName = account.Name;
                    user.PasswordHash = account.Password;
                    user.Email = account.Email;

                IdentityResult result = await manager.CreateAsync(user, account.Password);
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
