using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiJWTAuthentication.Authentication
{
    public interface IAuthenticationManager
    {
        string Authenticate(string userName, string password);
    }
}
