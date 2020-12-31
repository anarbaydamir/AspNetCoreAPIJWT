using ApiJWTAuthentication.Dto;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace ApiJWTAuthentication.Authentication
{
    public class RefreshTokenManager: IRefreshTokenManager
    {
        private readonly byte[] key;
        private readonly IAuthenticationManager authenticationManager;
        public RefreshTokenManager(byte[] key,IAuthenticationManager authenticationManager)
        {
            this.key = key;
            this.authenticationManager = authenticationManager;
        }
        public AuthenticationResponseDto refresh(RefreshCredentialsDto refreshCredentialsDto)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(refreshCredentialsDto.jwtToken,
                new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                }, out securityToken);
            var jwtSecuriteToken = securityToken as JwtSecurityToken;
            if(jwtSecuriteToken == null || jwtSecuriteToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature))
            {
                throw new SecurityTokenException("Invalid token");
            }

            var userName = principal.Identity.Name;
            if(refreshCredentialsDto.refreshToken != authenticationManager.userRefreshTokens[userName])
            {
                throw new SecurityTokenException("Invalid token");
            }

            return authenticationManager.Authenticate(userName, principal.Claims.ToArray());
        }
    }
}
