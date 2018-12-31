using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities
{
    public class User: IdentityUser
    {
        public string NickName { get; set; }
    }
}
