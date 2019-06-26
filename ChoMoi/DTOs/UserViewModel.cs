using Api.Models;
using DemoAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoAPI.ViewModels
{
    public class UserViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public UserViewModel(User user)
        {
            FirstName = user.FirstName;
            LastName = user.LastName;
            Username = user.UserName;
        }
    }
}
