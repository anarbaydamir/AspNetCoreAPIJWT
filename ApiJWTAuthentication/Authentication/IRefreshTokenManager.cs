using ApiJWTAuthentication.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiJWTAuthentication.Authentication
{
    public interface IRefreshTokenManager
    {
        AuthenticationResponseDto refresh(RefreshCredentialsDto refreshCredentialsDto);
    }
}
