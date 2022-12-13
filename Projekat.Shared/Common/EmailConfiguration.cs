using System;
using System.Collections.Generic;
using System.Text;

namespace Projekat.Shared.Common
{
    public class EmailConfiguration
    {
        public string Username { get; set; }
        public string ApiKey { get; set; }
        public string From { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
