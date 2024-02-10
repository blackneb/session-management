using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using session_management.Data;
using session_management.DTO;
using session_management.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace session_management.Controllers
{
    [EnableCors("AllowAnyOrigin")]
    [Route("api/[controller]")]
    [ApiController]
    public class KeyController : ControllerBase
    {
        private readonly SessionManagementDbContext _context;
        private readonly IMapper _mapper;

        public KeyController(SessionManagementDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<KeyModelDTO>>> GetKeys()
        {
            var keys = await _context.Keys.ToListAsync();

            var keyDtoList = keys.Select(key => _mapper.Map<KeyModelDTO>(key)).ToList();

            return Ok(keyDtoList);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<KeyModelDTO>> GetKeyById(int id)
        {
            var key = await _context.Keys.FindAsync(id);

            if (key == null)
            {
                return NotFound(); // Return 404 if the key is not found
            }

            var keyDto = _mapper.Map<KeyModelDTO>(key);

            return Ok(keyDto);
        }

        [HttpPost]
        public async Task<ActionResult<KeyModelDTO>> CreateKey(KeyModelDTO keyDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Generate a unique token as the key
            keyDto.KeyValue = GenerateUniqueKey();

            // Set the StartDate to the current UTC timestamp
            keyDto.StartDate = DateTime.UtcNow;

            // Set the ExpiryDate to one year from now in UTC
            keyDto.ExpiryDate = DateTime.UtcNow.AddYears(1);

            var key = _mapper.Map<KeyModel>(keyDto);
            _context.Keys.Add(key);
            await _context.SaveChangesAsync();

            var createdKeyDto = _mapper.Map<KeyModelDTO>(key);
            return CreatedAtAction(nameof(GetKeyById), new { id = createdKeyDto.KeyID }, createdKeyDto);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateKey(int id, KeyModelDTO keyDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingKey = await _context.Keys.FindAsync(id);

            if (existingKey == null)
            {
                return NotFound();
            }

            _mapper.Map(keyDto, existingKey);

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
        public async Task<IActionResult> DeleteKey(int id)
        {
            var key = await _context.Keys.FindAsync(id);

            if (key == null)
            {
                return NotFound();
            }

            _context.Keys.Remove(key);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Generate a unique token using JWT
        private string GenerateUniqueKey()
        {
            try
            {
                // Replace these values with your actual secret key, issuer, and audience
                var secretKey = "your-secret-key-with-sufficient-size";
                var issuer = "your-unique-issuer";
                var audience = "your-unique-audience";

                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new[]
                {
            new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString()), // Unique identifier for the token
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Unique token identifier
        };

                var token = new JwtSecurityToken(
                    issuer: issuer,
                    audience: audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddYears(1),
                    signingCredentials: credentials
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                // Handle the exception appropriately (log, rethrow, etc.)
                // Example: Log the exception and return a default or error value
                Console.WriteLine($"Error generating token: {ex.Message}");
                return "error-token";
            }
        }
    }
}
