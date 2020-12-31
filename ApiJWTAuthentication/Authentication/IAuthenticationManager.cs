using ApiJWTAuthentication.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ApiJWTAuthentication.Authentication
{
    public interface IAuthenticationManager
    {
        AuthenticationResponseDto Authenticate(string userName, string password);
        AuthenticationResponseDto Authenticate(string userName, Claim[] claims);
        public IDictionary<string, string> userRefreshTokens { get; set; }
    }
}
