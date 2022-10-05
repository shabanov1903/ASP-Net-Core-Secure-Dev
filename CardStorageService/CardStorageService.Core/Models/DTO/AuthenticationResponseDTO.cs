using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardStorageService.Core.Models.DTO
{
    public class AuthenticationResponseDTO
    {
        public AuthenticationStatus Status { get; set; }
        public SessionInfo SessionInfo { get; set; }
    }
}
