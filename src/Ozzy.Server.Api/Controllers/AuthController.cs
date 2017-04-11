using Microsoft.AspNetCore.Mvc;
using Ozzy.Server.Api.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ozzy.Server.Api.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private IAuthentificationService _authentificationService;

        public AuthController(IAuthentificationService authentificationService)
        {
            _authentificationService = authentificationService;
        }

        public async Task<bool> Login([FromBody]LoginModel data)
        {
            var rez = await _authentificationService.IsAuthorized(data.Login, data.Password);

            if (rez)
                await authenticate(data.Login);

            return rez;
        }

        public async Task Logout()
        {
           await HttpContext.Authentication.SignOutAsync("Cookies");
        }

        private async Task authenticate(string userName)
        {
            // создаем один claim
            var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
                };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.Authentication.SignInAsync("Cookies", new ClaimsPrincipal(id));
        }
    }
}
