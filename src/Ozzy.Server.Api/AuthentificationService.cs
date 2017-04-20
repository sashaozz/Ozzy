using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ozzy.Server.Api
{
    public interface IAuthentificationService
    {
        Task<bool> IsAuthorized(string login, string password);
    }

    public class AuthentificationService : IAuthentificationService
    {
        protected virtual List<Tuple<string, string>> _users { get; }
            = new List<Tuple<string, string>>() { new Tuple<string, string>("admin", "12345") }; 

        public async Task<bool> IsAuthorized(string login, string password)
        {
            var user = _users.FirstOrDefault(u => u.Item1 == login && u.Item2 == password);
            return user != null;
        }
    }
}
