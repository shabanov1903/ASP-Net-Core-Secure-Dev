using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardStorageService.Core.Models.DTO
{
    public class AuthenticationRequestDTO
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
