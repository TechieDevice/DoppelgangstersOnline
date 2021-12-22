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

        public UserAuthService(ApplicationContext context, IConfiguration conf)
        {
            _context = context;
            _appSettings = conf;
        }

        public string GetJwtToken(UserDto user)
        {
            var now = DateTime.UtcNow;
            var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.NickName),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.ToString())
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
            else user = ToDto(await _context.Users.FirstOrDefaultAsync(x => x.NickName == username && x.Password == password));

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

        public async Task<List<UserDto>> GetUsers()
        {
            var result = await _context.Users
                .AsNoTracking()
                .Select(cp => ToDto(cp))
                .ToListAsync();

            return result;
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
