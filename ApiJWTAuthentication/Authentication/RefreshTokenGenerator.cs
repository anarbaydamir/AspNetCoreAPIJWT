using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace ApiJWTAuthentication.Authentication
{
    public class RefreshTokenGenerator : IRefreshTokenGenerator
    {
        public string generateToken()
        {
            var randomNumber = new byte[32];
            using(var randomNumberGenerator = RandomNumberGenerator.Create())
            {
                randomNumberGenerator.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}
