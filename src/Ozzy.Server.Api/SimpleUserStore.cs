using Microsoft.AspNetCore.Identity;
using Ozzy.Server.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ozzy.Server.Api
{

    public class SimpleUserStore : 
                                     IUserPasswordStore<ApplicationUser>,
                                     IUserLoginStore<ApplicationUser>
     {
        private readonly List<ApplicationUser> _users;

        public SimpleUserStore()
        {
            _users = new List<ApplicationUser>();
        }

        public Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            user.UserId = Guid.NewGuid().ToString();

            _users.Add(user);

            return Task.FromResult(IdentityResult.Success);
        }

        public Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            var match = _users.FirstOrDefault(u => u.UserId == user.UserId);
            if (match != null)
            {
                match.UserName = user.UserId;
                match.Email = user.Email;
                match.PhoneNumber = user.PhoneNumber;
                match.PasswordHash = user.PasswordHash;

                return Task.FromResult(IdentityResult.Success);
            }
            else
            {
                return Task.FromResult(IdentityResult.Failed());
            }
        }

        public Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            var match = _users.FirstOrDefault(u => u.UserId == user.UserId);
            if (match != null)
            {
                _users.Remove(match);

                return Task.FromResult(IdentityResult.Success);
            }
            else
            {
                return Task.FromResult(IdentityResult.Failed());
            }
        }

        public Task<ApplicationUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            var user = _users.FirstOrDefault(u => u.UserId == userId);

            return Task.FromResult(user);
        }

        public Task<ApplicationUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            var user = _users.FirstOrDefault(u => u.UserId == normalizedUserName);

            return Task.FromResult(user);
        }

        public Task<string> GetUserIdAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserId);
        }

        public Task<string> GetUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserId);
        }

        public Task<string> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserId);
        }

        public Task<string> GetPasswordHashAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash != null);
        }

        public Task SetUserNameAsync(ApplicationUser user, string userName, CancellationToken cancellationToken)
        {
            user.UserId = userName;
            return Task.FromResult(true);
        }

        public Task SetNormalizedUserNameAsync(ApplicationUser user, string normalizedName, CancellationToken cancellationToken)
        {
            user.UserId = normalizedName;
            return Task.FromResult(true);
        }

        public Task SetPasswordHashAsync(ApplicationUser user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.FromResult(true);
        }
        public Task<IList<UserLoginInfo>> GetLoginsAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            // Just returning an empty list because I don't feel like implementing this. You should get the idea though...
            IList<UserLoginInfo> logins = new List<UserLoginInfo>();
            return Task.FromResult(logins);
        }

        public Task<ApplicationUser> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task AddLoginAsync(ApplicationUser user, UserLoginInfo login, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task RemoveLoginAsync(ApplicationUser user, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose() { }
    }
}
