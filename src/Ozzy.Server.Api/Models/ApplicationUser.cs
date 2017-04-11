using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Ozzy.Server.Api.Models
{
    public class ApplicationUser : ClaimsIdentity
    {
        public string UserId { get; set; }
        public string PasswordHash { get; set; }
    }
}
