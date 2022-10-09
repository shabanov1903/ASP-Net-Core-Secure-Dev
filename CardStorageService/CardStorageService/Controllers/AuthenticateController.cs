using CardStorageService.Core.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using CardStorageService.Core.Crypto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Net.Http.Headers;
using System.Net.Http.Headers;
using CardStorageService.Core.Models;
using FluentValidation;
using FluentValidation.Results;

namespace CardStorageService.Controllers
{
    [Authorize]
    [Route("api/auth")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IAuthenticateService _authenticateService;
        private readonly IValidator<AuthenticationRequestDTO> _authenticationValidator;

        public AuthenticateController(IAuthenticateService authenticateService, IValidator<AuthenticationRequestDTO> authenticationValidator)
        {
            _authenticateService = authenticateService;
            _authenticationValidator = authenticationValidator;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] AuthenticationRequestDTO authenticationRequest)
        {
            ValidationResult validationResult = _authenticationValidator.Validate(authenticationRequest);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.ToDictionary());

            AuthenticationResponseDTO authenticationResponse = _authenticateService.Login(authenticationRequest);
            if (authenticationResponse.Status == AuthenticationStatus.Success)
            {
                Response.Headers.Add("X-Session-Token", authenticationResponse.SessionInfo.SessionToken);
            }
            return Ok(authenticationResponse);
        }

        [HttpGet("session")]
        public IActionResult GetSessionInfo()
        {
            // Authorization : Bearer XXXXXXXXXXXXXXXXXXXXXXXX
            var authorization = Request.Headers[HeaderNames.Authorization];

            if (AuthenticationHeaderValue.TryParse(authorization, out var headerValue))
            {
                var scheme = headerValue.Scheme; // "Bearer"
                var sessionToken = headerValue.Parameter; // Token
                if (string.IsNullOrEmpty(sessionToken))
                    return Unauthorized();

                SessionInfo sessionInfo = _authenticateService.GetSessionInfo(sessionToken);
                if (sessionInfo == null)
                    return Unauthorized();

                return Ok(sessionInfo);
            }
            return Unauthorized();
        }
    }
}
