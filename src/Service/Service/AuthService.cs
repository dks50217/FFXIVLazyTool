using Core.Helper;
using Core.Helper.Core.Helper;
using Core.Model;
using Core.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public interface IAuthService
    {
        Task<bool> SignUp(string account, string totpkey);
        Task<bool> Login(string account, string password);
    }

    public class AuthService : IAuthService
    {
        private readonly HouseofsnowContext _context;

        public AuthService(HouseofsnowContext context)
        {
            _context = context;
        }

        public async Task<bool> Login(string account, string password)
        {
            var result = await _context.Users.Where(u => u.Name == account).FirstOrDefaultAsync();

            if (result == null)
            {
                return false;
            }

            var isSuccess = TOTPHelper.ValidateTotp(secret: result.TotpKey, code: password);

            return isSuccess;
        }

        public async Task<bool> SignUp(string account, string totpkey)
        {
            try
            {
                var result = await _context.Users.Where(u => u.Name == account).FirstOrDefaultAsync();

                if (result != null)
                {
                    return false;
                }

                User user = new User
                {
                    Name = account,
                    TotpKey = totpkey
                };

                await _context.Set<User>().AddAsync(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
