using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Models
{
    public class AppSettings
    {
        public string JwtKey { get; set; }
        public string JwtIssuer { get; set; }
        public int JwtExpireMinutes { get; set; }
    }
}
