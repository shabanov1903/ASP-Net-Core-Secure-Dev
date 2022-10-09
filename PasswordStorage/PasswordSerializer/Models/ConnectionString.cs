﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordSerializer.Models
{
    public class ConnectionString
    {
        public string Host { get; set; }

        public string DatabaseName { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public override string ToString()
        {
            return $"Host={Host}; Catalog={DatabaseName}; User={UserName}; Password={Password}";
        }
    }
}
