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
        private readonly ITokenService _tokenService;

        public AccountController(IAccountService accountService, ITokenService tokenService)
        {
            _accountService = accountService;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public ActionResult Register(RegisterDto registerDto)
        {
            var result = _accountService.Register(registerDto.Username.ToLower(), registerDto.Password);
            if(result.GetType() == typeof(AppUser))
            {
                return Ok
                    (
                        new UserDto
                        {
                            Username = result.Username,
                            Token = _tokenService.CreateToken(result)
                        }
                    );
            }
            else
            {
                return BadRequest("Username already exists.");
            }
        }


        [HttpPost("login")]
        public ActionResult Login(LoginDto loginDto)
        {
            try
            {
                AppUser appUser = _accountService.Login(loginDto);
                return Ok
                    (
                        new UserDto
                        {
                            Username = appUser.UserName,
                            Token = _tokenService.CreateToken(appUser)
                        }
                    );
            }
            catch (System.Web.Http.HttpResponseException exception)
            {
                return Unauthorized("Invalid Username or password. \n" + exception.Message);
            }
            catch(Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }
    }
}
