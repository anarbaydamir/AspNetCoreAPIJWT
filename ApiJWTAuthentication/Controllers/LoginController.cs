﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiJWTAuthentication.Authentication;
using ApiJWTAuthentication.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiJWTAuthentication.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly IAuthenticationManager authenticationManager;

        public LoginController(IAuthenticationManager authenticationManager)
        {
            this.authenticationManager = authenticationManager;
        }
        [HttpGet]
        public string Country()
        {
            return "Azerbaijan";
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authanticate([FromBody] UserCredentialsDto userCredential)
        {
            var token = authenticationManager.Authenticate(userCredential.userName, userCredential.password);
            if(token==null)
                return Unauthorized();
            return Ok(token);
        }
    }
}
