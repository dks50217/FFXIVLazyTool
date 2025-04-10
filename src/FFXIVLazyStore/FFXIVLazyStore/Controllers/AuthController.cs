using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Core.Model;
using Core.Service;
using Service.Service;
using Core.Helper;
using Core.Helper.Core.Helper;

namespace FFXIVLazyStore.Controllers
{
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly string _secretKey = "0+qp/XGGGi+nCoMjzSx0a8MZGA0KkDWTsI0Sewk2ZD8=";
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;


        public AuthController(IAuthService authService, ITokenService tokenService)
        {
            _authService = authService;
            _tokenService = tokenService;
        }

        [HttpGet("Signup")]
        public IActionResult InitSignup()
        {
            var secret = TOTPHelper.GenerateTotpSecret();
            var qrCode = TOTPHelper.GenerateQrCode(secret, "HouseOfSnow", "SnowTwoFactor");

            var result = new SignupResponse
            {
                Secret = secret,
                QrCode = qrCode
            };

            return Ok(result);
        }

        [HttpPost("Signup")]
        public async Task<IActionResult> Signup([FromBody] SignupRequest model)
        {
            var salt = PasswordHasherHelper.GenerateSalt();

            var hashPassword = PasswordHasherHelper.HashPassword(model.Password, salt);

            bool isSignup = await _authService.SignUp(model.Account, hashPassword);

            if (isSignup == false)
            {
                throw new Exception("Signup failed");
            }

            return Ok();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModelRequest model)
        {
            string issuer = model.Account;

            bool isLogin = await _authService.Login(model.Account, model.Password);

            if (isLogin == false)
            {
                throw new Exception("Login failed");
            }

            int expirationMinutes = 60;

            string token = _tokenService.GenerateToken(_secretKey, issuer, expirationMinutes);

            return Ok(new LoginModelResponse { Token = token });
        }
    }
}
