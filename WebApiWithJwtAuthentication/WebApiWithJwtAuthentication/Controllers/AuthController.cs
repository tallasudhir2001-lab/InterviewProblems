using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebApiWithJwtAuthentication.Data;
using WebApiWithJwtAuthentication.DTO;
using WebApiWithJwtAuthentication.Models;
using WebApiWithJwtAuthentication.Models.Identity;

namespace WebApiWithJwtAuthentication.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration configuration;
        private readonly ApplicationDbContext context;
        public AuthController(UserManager<ApplicationUser> userManager, IConfiguration configuration, ApplicationDbContext context)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.context = context;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO request)
        {
            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email
            };
            var result = await userManager.CreateAsync(user,request.Password);
            await userManager.AddToRoleAsync(user,"User");
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            return Ok("User Registered Successfully");
        }
        //[HttpPost("login")]
        //public async Task<IActionResult> Login(Login request)
        //{
        //    //if (request.Username != "admin" && request.Password != "1234")
        //    //{
        //    //    return Unauthorized("Invalid Credentials");
        //    //}
        //    //var claims = new[]
        //    //{
        //    //    new Claim(ClaimTypes.Name,request.Username),
        //    //    new Claim(ClaimTypes.Role,"Admin")
        //    //};
        //    //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("THIS_IS_A_VERY_SECRET_KEY_123456"));
        //    //var token = new JwtSecurityToken(
        //    //    issuer: "MyApi",
        //    //    audience: "MyClient",
        //    //    claims: claims,
        //    //    expires: DateTime.UtcNow.AddMinutes(30),
        //    //    signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));
        //    //var jwt= new JwtSecurityTokenHandler().WriteToken(token);
        //    //return Ok(new { token= jwt });
        //    var user = await userManager.FindByEmailAsync(request.Username);

        //    if (user == null)
        //        return Unauthorized();

        //    var valid = await userManager.CheckPasswordAsync(user, request.Password);

        //    if (!valid)
        //        return Unauthorized();

        //    return Ok("Login successful (Identity verified)");
        //}
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO request)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return Unauthorized("User not registered, Please register before logging in");
            }
            var isValid = await userManager.CheckPasswordAsync(user, request.Password);
            
            if (!isValid)
            {
                return Unauthorized("Invalid Credentials");
            }
            var token = await GenerateJwtToken(user);
            var refreshToken = new RefreshToken
            {
                Token = GenerateRefreshToken(),
                Expires = DateTime.UtcNow.AddDays(7),
                UserId = user.Id
            };
            context.RefreshTokens.Add(refreshToken);
            await context.SaveChangesAsync();
            //this code will send it to browser without the method returning it, sent along with token in Header
            Response.Cookies.Append("refresh_Token", refreshToken.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = refreshToken.Expires
            });
            return Ok(new { token });
        }
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refresh_Token"];
            if(refreshToken == null)
            {
                return Unauthorized();
            }
            var token = await context.RefreshTokens.Include(rt => rt.User).SingleOrDefaultAsync(rt => rt.Token == refreshToken);
            if(token == null || !token.IsActive)
            {
                return Unauthorized();
            }
            token.Revoked= DateTime.UtcNow;
            var newRefreshToken = new RefreshToken
            {
                Token = GenerateRefreshToken(),
                Expires = DateTime.UtcNow.AddDays(7),
                UserId = token.UserId
            };
            context.RefreshTokens.Add(newRefreshToken);
            var newJwt = await GenerateJwtToken(token.User);
            await context.SaveChangesAsync();
            Response.Cookies.Append("refresh_Token", newRefreshToken.Token, new CookieOptions//existing with same name is overwritten
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = newRefreshToken.Expires
            });
            return Ok(new { newJwt });
        }
        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var refreshToken = Request.Cookies["refresh_Token"];
            var token = await context.RefreshTokens.SingleOrDefaultAsync(rt => rt.Token == refreshToken);
            if(token != null)
            {
                token.Revoked= DateTime.UtcNow;
                await context.SaveChangesAsync();
            }
            Response.Cookies.Delete("refresh_Token");
            return Ok();
        }
        private async Task<string> GenerateJwtToken(ApplicationUser user)
        {
            var roles = await userManager.GetRolesAsync(user);
            var claims = new List<Claim>{
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            foreach(var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration["Jwt:Key"])
            );
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                Convert.ToDouble(configuration["Jwt:ExpiresInMinutes"])
                ),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private static string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }
}
