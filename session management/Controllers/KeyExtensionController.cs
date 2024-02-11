using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using session_management.Data;
using session_management.DTO;
using session_management.Models;
using System;
using System.Threading.Tasks;

namespace session_management.Controllers
{
    [EnableCors("AllowAnyOrigin")]
    [Route("api/[controller]")]
    [ApiController]
    public class KeyExtensionController : ControllerBase
    {
        private readonly SessionManagementDbContext _context;
        private readonly IMapper _mapper;

        public KeyExtensionController(SessionManagementDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<KeyExtensionModelDTO>> GetKeyExtension(int id)
        {
            var keyExtension = await _context.KeyExtensions
                .Include(ke => ke.Key) // Include related Key information if needed
                .FirstOrDefaultAsync(ke => ke.ExtensionID == id);

            if (keyExtension == null)
            {
                return NotFound(); // Return 404 if the key extension is not found
            }

            var keyExtensionDto = _mapper.Map<KeyExtensionModelDTO>(keyExtension);
            return Ok(keyExtensionDto);
        }

        [HttpPost]
        public async Task<ActionResult<KeyExtensionModelDTO>> CreateKeyExtension([FromBody] KeyExtensionModelDTO keyExtensionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Retrieve KeyModel by KeyID
            var keyModel = await _context.Keys.FindAsync(keyExtensionDto.KeyID);

            if (keyModel == null)
            {
                return NotFound("Key not found");
            }

            // Calculate NewExpiryDate by adding 1 year to the ExpiryDate
            keyExtensionDto.NewExpiryDate = keyModel.ExpiryDate.AddYears(1);

            // Set ExtensionDate to the current timestamp
            keyExtensionDto.ExtensionDate = DateTime.UtcNow;

            var keyExtension = _mapper.Map<KeyExtensionModel>(keyExtensionDto);

            _context.KeyExtensions.Add(keyExtension);
            await _context.SaveChangesAsync();

            var createdKeyExtensionDto = _mapper.Map<KeyExtensionModelDTO>(keyExtension);
            return CreatedAtAction(nameof(GetKeyExtension), new { id = createdKeyExtensionDto.ExtensionID }, createdKeyExtensionDto);
        }

        [HttpGet("check-expiration/{keyId}")]
        public async Task<IActionResult> CheckKeyExpiration(int keyId)
        {
            var result = await CheckKeyExpirationInternal(keyId);
            return result;
        }

        [HttpGet("check-expiration-by-value/{keyValue}")]
        public async Task<IActionResult> CheckKeyExpirationByValue(string keyValue)
        {
            // Find the key by keyValue
            var keyModel = await _context.Keys.FirstOrDefaultAsync(k => k.KeyValue == keyValue);

            if (keyModel != null)
            {
                var result = await CheckKeyExpirationInternal(keyModel.KeyID);
                return result;
            }
            else
            {
                return NotFound("Key not found");
            }
        }

        private async Task<IActionResult> CheckKeyExpirationInternal(int keyId)
        {
            var keyExtension = await _context.KeyExtensions
                .Include(ke => ke.Key) // Include related Key information if needed
                .FirstOrDefaultAsync(ke => ke.KeyID == keyId);

            if (keyExtension != null)
            {
                // Check if the key extension is expired
                if (keyExtension.NewExpiryDate < DateTime.UtcNow)
                {
                    return Ok(new { Expired = true, ExpiryDate = keyExtension.NewExpiryDate });
                }
                else
                {
                    return Ok(new { Expired = false, ExpiryDate = keyExtension.NewExpiryDate });
                }
            }
            else
            {
                // If keyId is not found in KeyExtensionModel, check in KeyModel
                var keyModel = await _context.Keys.FindAsync(keyId);

                if (keyModel != null)
                {
                    // Check if the key in KeyModel is expired
                    if (keyModel.ExpiryDate < DateTime.UtcNow)
                    {
                        return Ok(new { Expired = true, ExpiryDate = keyModel.ExpiryDate });
                    }
                    else
                    {
                        return Ok(new { Expired = false, ExpiryDate = keyModel.ExpiryDate });
                    }
                }
                else
                {
                    return NotFound("Key not found");
                }
            }
        }
    }
}
