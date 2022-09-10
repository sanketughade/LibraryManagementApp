using Library_Management_App.DTOs;
using Library_Management_App.Interfaces;
using Library_Management_App.Models;
using Library_Management_App.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management_App.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("register")]
        public ActionResult Register(RegisterDto registerDto)
        {
            return Ok(_accountService.Register(registerDto.Username.ToLower(), registerDto.Password));
        }
    }
}
