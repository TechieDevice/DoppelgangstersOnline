using DoppelgangstersOnline.Database;
using DoppelgangstersOnline.Database.Models;
using DoppelgangstersOnline.Dtos;
using DoppelgangstersOnline.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace DoppelgangstersOnline.Services
{
    public class UserAuthService : IUserService
    {
        private ApplicationContext _context;
        private readonly IConfiguration _appSettings;
        private readonly IDictionary<string, Player> _clients;

        public UserAuthService(ApplicationContext context, IConfiguration conf, IDictionary<string, Player> clients)
        {
            _context = context;
            _appSettings = conf;
            _clients = clients;
        }

        public IEnumerable<GetUsersDto> GetUsers()
        {
            var clients = _clients.ToArray();
            return Enumerable.Range(0, clients.Length).Select(index => new GetUsersDto
            {
                NickName = clients[index].Value.NickName,
                Room = clients[index].Value.RoomId
            })
            .ToArray();
        }

        public string GetJwtToken(UserDto user)
        {
            var now = DateTime.UtcNow;
            var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.NickName),
                    new Claim(ClaimTypes.Role, user.Role)
                };
            var identity = new ClaimsIdentity(claims, "Token",
                                    ClaimsIdentity.DefaultNameClaimType,
                                    ClaimsIdentity.DefaultRoleClaimType);
            
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }

        public async Task<UserDto> GetUser(string username, string password)
        {
            var user = new UserDto();
            var userM = new User();

            var login = _appSettings.GetSection("AdminLogin").GetSection("l").Value;
            var pass = _appSettings.GetSection("AdminLogin").GetSection("p").Value;

            if (login == username && pass == password)
            {
                user = new UserDto
                {
                    Id = 0,
                    NickName = login,
                    Password = string.Empty,
                    Role = "Admin"
                };
            }
            else
            {
                userM = await _context.Users.FirstOrDefaultAsync(x => x.NickName == username && x.Password == password);
                if (userM == null) return null;
                user = ToDto(userM);
            }

            if (user == null) return null;
            return user;
        }

        public async Task<UserDto> AddUser(UserDto userDto)
        {
            if (userDto == null) return null;

            var user = ToModel(userDto);
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();

            return userDto;
        }

        private User ToModel(UserDto userDto)
        {
            return new User
            {
                Password = userDto.Password,
                Role = userDto.Role,
                NickName = userDto.NickName
            };
        }

        private static UserDto ToDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Password = user.Password,
                Role = user.Role,
                NickName = user.NickName
            };
        }
    }
}
