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
        Task<bool> SignUp(string account, string password);
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

            var isSuccess = PasswordHasherHelper.VerifyPassword(password, result.Password);

            return isSuccess;
        }

        public async Task<bool> SignUp(string account, string password)
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
                    Password = password
                };

                await _context.Set<User>().AddAsync(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}
