using CardStorageService.Core.Models;
using CardStorageService.Core.Models.DTO;
using CardStorageService.DataBase;
using CardStorageService.DataBase.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CardStorageService.Core.Crypto.Impl
{
    public class AuthenticateService : IAuthenticateService
    {
        #region Services

        private readonly IServiceScopeFactory _serviceScopeFactory;

        #endregion

        public const string SecretKey = "kYp3s6v9y/B?E(H+";

        private readonly Dictionary<string, SessionInfo> _sessions =
            new Dictionary<string, SessionInfo>();

        public AuthenticateService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public SessionInfo GetSessionInfo(string sessionToken)
        {
            SessionInfo sessionInfo;

            lock (_sessions)
            {
                _sessions.TryGetValue(sessionToken, out sessionInfo);
            }

            if (sessionInfo == null)
            {
                using IServiceScope scope = _serviceScopeFactory.CreateScope();
                CardStorageServiceDbContext context = scope.ServiceProvider.GetRequiredService<CardStorageServiceDbContext>();

                AccountSession session = context.AccountSessions.FirstOrDefault(item => item.SessionToken == sessionToken);

                if (session == null)
                    return null;

                Account account = context.Accounts.FirstOrDefault(item => item.AccountId == session.AccountId);

                sessionInfo = GetSessionInfo(account, session);

                if (sessionInfo != null)
                {
                    lock (_sessions)
                    {
                        _sessions[sessionToken] = sessionInfo;
                    }
                }
            }
            return sessionInfo;
        }

        public AuthenticationResponseDTO Login(AuthenticationRequestDTO authenticationRequest)
        {
            using IServiceScope scope = _serviceScopeFactory.CreateScope();
            CardStorageServiceDbContext context = scope.ServiceProvider.GetRequiredService<CardStorageServiceDbContext>();
            ICrypto crypto = scope.ServiceProvider.GetRequiredService<CryptoFatotySHA512>();

            Account account =!string.IsNullOrWhiteSpace(authenticationRequest.Login) ? FindAccountByLogin(context, authenticationRequest.Login) : null;

            if (account == null)
            {
                return new AuthenticationResponseDTO
                {
                    Status = AuthenticationStatus.UserNotFound
                };
            }

            if (!crypto.VerifyPassword(authenticationRequest.Password, account.PasswordSalt, account.PasswordHash))
            {
                return new AuthenticationResponseDTO
                {
                    Status = AuthenticationStatus.InvalidPassword
                };
            }

            AccountSession session = new AccountSession
            {
                AccountId = account.AccountId,
                SessionToken = CreateSessionToken(account),
                TimeCreated = DateTime.Now,
                TimeLastRequest = DateTime.Now,
                IsClosed = false,
            };

            context.AccountSessions.Add(session);

            context.SaveChanges();

            SessionInfo sessionInfo = GetSessionInfo(account, session);

            lock (_sessions)
            {
                _sessions[sessionInfo.SessionToken] = sessionInfo;
            }

            return new AuthenticationResponseDTO
            {
                Status = AuthenticationStatus.Success,
                SessionInfo = sessionInfo
            };
        }

        private SessionInfo GetSessionInfo(Account account, AccountSession accountSession)
        {
            return new SessionInfo
            {
                SessionId = accountSession.SessionId,
                SessionToken = accountSession.SessionToken,
                Account = new AccountInfo
                {
                    AccountId = account.AccountId,
                    EMail = account.EMail,
                    FirstName = account.FirstName,
                    LastName = account.LastName,
                    SecondName = account.SecondName,
                    Locked = account.Locked
                }
            };
        }

        private string CreateSessionToken(Account account)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(SecretKey);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new Claim[]{
                        new Claim(ClaimTypes.NameIdentifier, account.AccountId.ToString()),
                        new Claim(ClaimTypes.Email, account.EMail),
                    }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private Account FindAccountByLogin(CardStorageServiceDbContext context, string login)
        {
            return context.Accounts.FirstOrDefault(account => account.EMail == login);
        }
    }
}
