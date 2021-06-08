using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebGallery.Entities.Identity;

namespace WebAuction.Services
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly UserManager<AppUser> _userManager;
        public JwtTokenService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public string CreateToken(AppUser user)
        {
            var roles = _userManager.GetRolesAsync(user).Result;
            roles = roles.OrderBy(x => x).ToList();

            List<Claim> claims = new List<Claim>()
            {
                new Claim("id",user.Id.ToString()),
                new Claim("name",user.UserName)
            };
            foreach (var role in roles)
            {
                claims.Add(new Claim("roles", role));
            }

            var signinKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Tra ta ta asiekz8w83sadflk09234rasdfWWwed5462345"));//ключ шифрования
            var signinCredentials = new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                signingCredentials: signinCredentials,
                expires: DateTime.Now.AddDays(100),//срок авторизации
                claims: claims
                );
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
