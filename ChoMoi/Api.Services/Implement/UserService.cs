using System;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Api.Models;
using ChoMoi.Api.Services.Implement;
using ChoMoiApi.DTOs;
using DemoAPI.Helper;
using DemoAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ChoMoi.Api.Services.Implement
{

    public class UserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        readonly BookStoreContext bookStoreContext;

        public UserService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            BookStoreContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            bookStoreContext = context;
        }

        public async Task<ErrorModel> RegisterNewUser(User newUser, string password, String role)
        {
            

            IdentityResult userCreationResult = null;
            IdentityResult userAddRoleResult = null;
            try
            {
                userCreationResult = await _userManager.CreateAsync(newUser, password);
                userAddRoleResult = await _userManager.AddToRoleAsync(newUser, role);
            }
            catch (SqlException)
            {
                var error = new ErrorModel("Error communicating with the database, see logs for more detailsPasswords don't match!", HttpStatusCode.InternalServerError);
                return error;
            }

            if (!userCreationResult.Succeeded)
            {
                var error = new ErrorModel("An error occurred when creating the user, see nested errors", HttpStatusCode.BadRequest);
                return error;

            }
            if (!userAddRoleResult.Succeeded)
            {
                var error = new ErrorModel("An error occurred when creating role for user, see nested errors", HttpStatusCode.BadRequest);
                return error;

            }

            //var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);

            //var tokenVerificationUrl = Url.Action(
            //     "VerifyEmail", "Account",
            //     new
            //     {
            //         Id = newUser.Id,
            //         token = emailConfirmationToken
            //     },
            //     Request.Scheme);

            //var erro = new ErrorModel($"Registration completed, please verify your email - {email}", HttpStatusCode.OK);
            var erro = new ErrorModel($"Registration completed", HttpStatusCode.OK);
            return erro;

        }

        public async Task<ErrorModel> LoginService(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                var error = new ErrorModel("email or password is null", HttpStatusCode.BadRequest);
                return error;

            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                var error = new ErrorModel("Invalid Login and/or password", HttpStatusCode.BadRequest);
                return error;

            }

            if (!user.EmailConfirmed)
            {
                var error = new ErrorModel("Email not confirmed, please check your email for confirmation link", HttpStatusCode.BadRequest);
                return error;

            }

            var passwordSignInResult = await _signInManager.PasswordSignInAsync(user, password, isPersistent: true, lockoutOnFailure: false);
            if (!passwordSignInResult.Succeeded)
            {
                var error = new ErrorModel("Invalid Login and/or password", HttpStatusCode.BadRequest);
                return error;

            }
            //var erro = new Error("Cookie created", HttpStatusCode.OK);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes("23hfv2v0bn9290h29bbg20b0");
            var roleList = await _userManager.GetRolesAsync(user);
            var userRole = roleList.FirstOrDefault();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("ID", user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName.ToString()),
                    new Claim(ClaimTypes.Role, userRole.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                Issuer = "vu@email.com",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);
            await _userManager.UpdateAsync(user);

            var erro = new ErrorModel(tokenHandler.WriteToken(token), HttpStatusCode.OK);

            return erro;
        }


        public async Task<ErrorModel> LogOutService()
        {
            await _signInManager.SignOutAsync();

            var error = new ErrorModel("You have been successfully logged out", HttpStatusCode.OK);
            return error;
        }

        public async Task<User> GetUserByToken(string token)
        {
            await _signInManager.SignOutAsync();
            var user = bookStoreContext.Users.FirstOrDefault(x => "Bearer "+x.Token  == token);
            if (user != null)
                return user;
            else return null;
        }
    }
}
