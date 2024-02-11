using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using session_management.Data;
using session_management.DTO;
using session_management.Models;
using System.Linq;
using System.Threading.Tasks;

namespace session_management.Controllers
{
    [EnableCors("AllowAnyOrigin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserKeyController : ControllerBase
    {
        private readonly SessionManagementDbContext _context;
        private readonly IMapper _mapper;

        public UserKeyController(SessionManagementDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserKeyModelDTO>> GetUserKey(int id)
        {
            var userKey = await _context.UserKeys
                .Include(uk => uk.Key) // Include related Key information if needed
                .FirstOrDefaultAsync(uk => uk.UserKeyID == id);

            if (userKey == null)
            {
                return NotFound(); // Return 404 if the user key is not found
            }

            var userKeyDto = _mapper.Map<UserKeyModelDTO>(userKey);
            return Ok(userKeyDto);
        }

        [HttpPost]
        public async Task<ActionResult<UserKeyModelDTO>> CreateUserKey([FromBody] UserKeyModelValueDTO userKeyDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if the user exists
            var user = await _context.Users.FindAsync(userKeyDto.UserID);

            if (user == null)
            {
                return NotFound("User not found");
            }

            // Find the key by keyValue
            var key = await _context.Keys.FirstOrDefaultAsync(k => k.KeyValue == userKeyDto.KeyValue);

            if (key == null)
            {
                return NotFound("Key not found");
            }

            // Check the total number of keys with the same KeyId for the user
            var totalKeysWithSameKeyId = _context.UserKeys.Count(uk => uk.UserID == userKeyDto.UserID && uk.KeyID == key.KeyID);

            // Check if the total exceeds or is equal to 3
            if (totalKeysWithSameKeyId >= 3)
            {
                return BadRequest("Maximum limit for KeyId exceeded (greater than or equal to 3).");
            }

            var userKey = new UserKeyModel
            {
                UserID = user.UserID,
                KeyID = key.KeyID,
                MachinesUsed = userKeyDto.MachinesUsed
            };

            _context.UserKeys.Add(userKey);
            await _context.SaveChangesAsync();

            var createdUserKeyDto = _mapper.Map<UserKeyModelDTO>(userKey);
            return CreatedAtAction(nameof(GetUserKey), new { id = createdUserKeyDto.UserKeyID }, createdUserKeyDto);
        }

    }
}
