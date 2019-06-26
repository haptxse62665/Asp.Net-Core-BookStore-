using System;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Api.Models;
using ChoMoi.Api.Services.Implement;
using ChoMoi.Helper;
using ChoMoiApi.DTOs;
using DemoAPI.Helper;
using DemoAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;



namespace DemoAPI.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly UserService _userService;
        readonly BookStoreContext bookStoreContext;

        public UsersController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            UserService userService,
            BookStoreContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            bookStoreContext = context;
            _userService = userService;
        }

        /// <summary>
        /// Register
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="confirmPassword"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("register")]
        public async Task<JsonResult> Register(string email, string password, string confirmPassword, string role)
        {
            if (!ObjectFields.ROLES.Contains(role)) {
                var error = new ErrorModel("role is wwrong", HttpStatusCode.BadRequest);
                return new JsonResult(error) { StatusCode = error.StatusCode };
            } 
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                var error = new ErrorModel("email or password is null", HttpStatusCode.BadRequest);
                return new JsonResult(error) { StatusCode = error.StatusCode };
               
            }

            if (password != confirmPassword)
            {
                var error = new ErrorModel("Passwords don't match!", HttpStatusCode.BadRequest);
                return new JsonResult(error) { StatusCode = error.StatusCode };
            }

            var newUser = new User
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true,
            };

            var result = await _userService.RegisterNewUser(newUser, password, role);

            return new JsonResult(result);


        }

        //public async Task<IActionResult> VerifyEmail(string id, string token)
        //{
        //    var user = await _userManager.FindByIdAsync(id);
        //    if (user == null)
        //        throw new InvalidOperationException();

        //    var emailConfirmationResult = await _userManager.ConfirmEmailAsync(user, token);
        //    if (!emailConfirmationResult.Succeeded)
        //    {
        //        var erro = new Error("Fail", HttpStatusCode.BadRequest);
        //        return new JsonResult(erro) { StatusCode = erro.StatusCode };
        //    }
        //    var error = new Error("OK", HttpStatusCode.OK);
        //    return new JsonResult(error) { StatusCode = error.StatusCode };
        //}
        /// <summary>
        /// Login
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        public async Task<JsonResult> Login(string email, string password)
        {
            var result = await _userService.LoginService(email, password);
            return new JsonResult(result);

        }

        /// <summary>
        /// Logout Cookie
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("logout")]
        public async Task<JsonResult> Logout()
        {
            var result =await _userService.LogOutService();
            return new JsonResult(result);

        }

        /// <summary>
        /// Check Token
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("token")]
        public async Task<JsonResult> GetUserbyTokenAsync()
        {
            string token = Request.Headers["Authorization"];
            var user = await _userService.GetUserByToken(token);
            if(user != null)
                return new JsonResult(user);
            var error = new ErrorModel("Token not found!", HttpStatusCode.BadRequest);
            return new JsonResult(error) { StatusCode = error.StatusCode };
        }
    }


   
}