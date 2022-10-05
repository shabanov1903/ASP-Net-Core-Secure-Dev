using CardStorageService.Core.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardStorageService.Core.Crypto
{
    public interface IAuthenticateService
    {
        public AuthenticationResponseDTO Login(AuthenticationRequestDTO authenticationRequest);
        public SessionInfo GetSessionInfo(string sessionToken);
    }
}
