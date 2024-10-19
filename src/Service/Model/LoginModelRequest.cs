using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    public class LoginModelRequest
    {
        public required string Account { get; set; }
        public required string Password { get; set; }
    }

    public class LoginModelResponse
    {
        public string Token { get; set; }
    }

    public class SignupRequest
    {
        public required string Account { get; set; }
        public required string Password { get; set; }
        public required string Secret { get; set; }  
    }

    public class SignupResponse
    {
        public required string Secret { get; set; }
        public required byte[] QrCode { get; set; }
    }
}
