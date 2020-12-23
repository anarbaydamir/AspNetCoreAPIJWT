using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ApiJWTAuthentication.Authentication
{
    public class AuthentionManagerManager : IAuthenticationManager
    {
        private readonly IDictionary<string, string> users = new Dictionary<string, string>
        { {"user1","pass1"},{ "user2","pass2"} };
        private readonly string key;
        public AuthentionManagerManager(string key)
        {
            this.key = key;
        }
        public string Authenticate(string userName, string password)
        {
            if(!users.Any(u=>u.Key == userName && u.Value  == password))
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userName)
                }),
                Expires = DateTime.Now.AddMinutes(1),
                SigningCredentials = new SigningCredentials
                (new SymmetricSecurityKey(tokenKey),
                SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
