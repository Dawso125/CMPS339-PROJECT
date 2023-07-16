using CMPS339_PROJECT.Models;
using CMPS339_PROJECT.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using CMPS339_PROJECT.Services;
using System.Data.SqlClient;
using System.Data;

namespace CMPS339.Controllers
{
    [ApiController]
    [Route("/api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;


        public UserController(ILogger<UserController> logger, IUserService userService)
        {
            _userService = userService;
            _logger = logger;

        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            List<Users> Users = await _userService.GetAllAsync();

            return Ok(Users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            Users? user = await _userService.GetByIdAsync(id);

            if (user != null)
            {
                return Ok(user);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> Create(UsersCreateDto dto)
        {
            if (ModelState.IsValid)
            {
                UsersGetDto? users = await _userService.InsertAsync(dto);

                if (users != null)
                {
                    return Ok(users);
                }
                return BadRequest("Unable to insert the record");
            }
            return BadRequest("The model is invalid");


        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteById(int id)
        {
            UsersGetDto? user = await _userService.DeleteByIdAsync(id);

            if (user != null)
            {
                return Ok(user);
            }
            return NotFound();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] UsersCreateUpdateDto dto)
        {
            var userToEdit = await _userService.GetByIdAsync(id);

            if (userToEdit != null)
            {
                if (ModelState.IsValid)
                {
                    var updatedUser = await _userService.UpdateAsync(id, dto);

                    if (updatedUser != null)
                    {
                        var userGetDto = new UsersGetDto
                        {
                            Id = updatedUser.Id,
                            Username = updatedUser.Username,
                            Password = updatedUser.Password,
                            IsActive = updatedUser.IsActive,
                        };

                        return Ok(userGetDto);
                    }

                    return BadRequest("Unable to update the user.");
                }

                return BadRequest("The model is invalid.");
            }

            return NotFound("Could not find user to update.");
        }
    }
}