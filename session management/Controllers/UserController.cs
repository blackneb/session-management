using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using session_management.Data;
using session_management.DTO;
using session_management.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace session_management.Controllers
{
    [EnableCors("AllowAnyOrigin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly SessionManagementDbContext _context;
        private readonly IMapper _mapper;

        public UserController(SessionManagementDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserModelDTO>>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            var userDtos = _mapper.Map<IEnumerable<UserModelDTO>>(users);

            return Ok(userDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserModelDTO>> GetUserById(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound(); // Return 404 if the user is not found
            }

            var userDto = _mapper.Map<UserModelDTO>(user);
            return Ok(userDto);
        }

        [HttpPost]
        public async Task<ActionResult<UserModelDTO>> CreateUser(UserModelDTO userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _mapper.Map<UserModel>(userDto);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var createdUserDto = _mapper.Map<UserModelDTO>(user);
            return CreatedAtAction(nameof(GetUserById), new { id = createdUserDto.UserID }, createdUserDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserModelDTO userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingUser = await _context.Users.FindAsync(id);

            if (existingUser == null)
            {
                return NotFound();
            }

            _mapper.Map(userDto, existingUser);

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
