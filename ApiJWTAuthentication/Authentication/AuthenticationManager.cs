using ApiJWTAuthentication.Dto;
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
    public class AuthenticationManager : IAuthenticationManager
    {
        private readonly IDictionary<string, string> users = new Dictionary<string, string>
        { {"user1","pass1"},{ "user2","pass2"} };

        public IDictionary<string, string> userRefreshTokens { get; set; }

        private readonly string key;
        private readonly IRefreshTokenGenerator refreshTokenGenerator;

        public AuthenticationManager(string key,IRefreshTokenGenerator refreshTokenGenerator)
        {
            this.key = key;
            this.refreshTokenGenerator = refreshTokenGenerator;
            this.userRefreshTokens = new Dictionary<string,string>();
        }

        public AuthenticationResponseDto Authenticate(string userName, string password)
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
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = new SigningCredentials
                (new SymmetricSecurityKey(tokenKey),
                SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var refreshToken = refreshTokenGenerator.generateToken();

            if (userRefreshTokens.ContainsKey(userName))
                userRefreshTokens[userName] = refreshToken;
            else
                userRefreshTokens.Add(userName, refreshToken);

            return new AuthenticationResponseDto
            {
                jwtToken = tokenHandler.WriteToken(token),
                refreshToken = refreshToken
            };
        }

        public AuthenticationResponseDto Authenticate(string userName,Claim[] claims)
        {
            var tokenKey = Encoding.ASCII.GetBytes(key);
            var jwtSecurityToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(1),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature)
                );

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            var refreshToken = refreshTokenGenerator.generateToken();

            if (userRefreshTokens.ContainsKey(userName))
                userRefreshTokens[userName] = refreshToken;
            else
                userRefreshTokens.Add(userName, refreshToken);

            return new AuthenticationResponseDto
            {
                jwtToken = jwtToken,
                refreshToken = refreshToken
            };
        }
    }
}
