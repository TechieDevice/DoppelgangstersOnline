using DoppelgangstersOnline.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoppelgangstersOnline.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> AddUser(UserDto userDto);

        Task<UserDto> GetUser(string username, string password);

        string GetJwtToken(UserDto user);

        IEnumerable<GetUsersDto> GetUsers();
    }
}
