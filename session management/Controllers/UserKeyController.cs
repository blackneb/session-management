using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet("key-info/{keyId}")]
        public ActionResult<KeyInfoDTO> GetKeyInfo(int keyId)
        {
            // Find the key by keyId
            var key = _context.Keys.FirstOrDefault(k => k.KeyID == keyId);

            if (key == null)
            {
                return NotFound("Key not found");
            }

            // Calculate the total used machines for the key
            var totalUsedMachines = _context.UserKeys
                .Where(uk => uk.KeyID == keyId)
                .Sum(uk => uk.MachinesUsed);

            // Calculate the left machines for the key
            var leftMachines = key.MaxMachines - totalUsedMachines;

            // Retrieve the list of user ids for the key
            var userIds = _context.UserKeys
                .Where(uk => uk.KeyID == keyId)
                .Select(uk => uk.UserID)
                .ToList();

            var keyInfoDto = new KeyInfoDTO
            {
                MaxMachines = key.MaxMachines,
                UsedMachines = totalUsedMachines,
                LeftMachines = leftMachines,
                UserIDs = userIds
            };

            return Ok(keyInfoDto);
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

            // Check the total number of machines used by the user for the key
            var totalMachinesUsed = _context.UserKeys
                .Where(uk => uk.UserID == userKeyDto.UserID && uk.KeyID == key.KeyID)
                .Sum(uk => uk.MachinesUsed);

            // Check if the total exceeds the maxMachines limit
            if (totalMachinesUsed + 1 > key.MaxMachines)
            {
                return BadRequest($"Total machines used exceeds the limit of {key.MaxMachines} for the specified key.");
            }

            // Increment the MachinesUsed property by 1
            var userKey = new UserKeyModel
            {
                UserID = user.UserID,
                KeyID = key.KeyID,
                MachinesUsed = 1 // Increment by 1
            };

            _context.UserKeys.Add(userKey);
            await _context.SaveChangesAsync();

            var createdUserKeyDto = _mapper.Map<UserKeyModelDTO>(userKey);
            return CreatedAtAction(nameof(GetUserKey), new { id = createdUserKeyDto.UserKeyID }, createdUserKeyDto);
        }



    }
}
