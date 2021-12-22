using DoppelgangstersOnline.Dtos;
using DoppelgangstersOnline.Services;
using DoppelgangstersOnline.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DoppelgangstersOnline.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private IUserService _userService;
        public UserController(IUserService service)
        {
            _userService = service;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Post(UserDto userDto)
        {
            userDto.Role = "User";

            await _userService.AddUser(userDto);

            return Ok(userDto);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Token(UserDto userDto)
        {
            var user = await _userService.GetUser(userDto.NickName, userDto.Password);
            if (user == null)
            {
                return BadRequest(new { errorText = "Invalid username or password." });
            }

            var encodedJwt = _userService.GetJwtToken(user);

            // Ответ на фронт
            var response = new
            {
                token = encodedJwt,
                user = new
                {
                    id = user.Id.ToString(),
                    nickName = user.NickName,
                    role = user.Role
                }
            };

            return Json(response);
        }
    }
}
