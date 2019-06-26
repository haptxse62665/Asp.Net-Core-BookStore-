using DemoAPI.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }


        public string Token { get; set; }
        public bool Deleted { get; set; }

        public virtual AuthorContact AuthorContact { get; set; }
        public virtual ICollection<BookAuthors> BookAuthors { get; set; }

        public User() : base()
        {
            FirstName = LastName = Token = "";
            Deleted = false;
        }
        public User(string userName) : base(userName) { }

    }
}
