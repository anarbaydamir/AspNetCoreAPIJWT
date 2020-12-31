using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiJWTAuthentication.Dto
{
    public class AuthenticationResponseDto
    {
        public string jwtToken { get; set; }
        public string refreshToken { get; set; }
    }
}
