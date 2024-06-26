﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using session_management.Data;
using session_management.DTO;
using session_management.Models;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
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
        [Authorize]
        public async Task<ActionResult<IEnumerable<UserResponseDTO>>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();

            var userResponseDtos = users.Select(user => new UserResponseDTO
            {
                UserID = user.UserID,
                Username = user.Username,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                IsBlocked = user.IsBlocked,
                IsEmailVerified = user.IsEmailVerified
            }).ToList();

            return Ok(userResponseDtos);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<UserResponseDTO>> GetUserById(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound(); // Return 404 if the user is not found
            }

            var userResponseDto = new UserResponseDTO
            {
                UserID = user.UserID,
                Username = user.Username,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                IsBlocked = user.IsBlocked,
                IsEmailVerified = user.IsEmailVerified
            };

            return Ok(userResponseDto);
        }


        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateUser(int id, UserModelUpdateDTO userDto)
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

            // Check if the provided phone number is already used by another user
            if (await _context.Users.AnyAsync(u => u.UserID != id && u.PhoneNumber == userDto.PhoneNumber))
            {
                return BadRequest("Phone number is already in use by another user.");
            }

            _mapper.Map(userDto, existingUser);

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { message = "User updated successfully" });
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500);
            }
        }


        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "User deleted successfully" });
        }


        [HttpPatch("{id}/block")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> BlockUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            user.IsBlocked = !user.IsBlocked; // Toggle the IsBlocked property

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { IsBlocked = user.IsBlocked });
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500);
            }
        }

        [HttpPatch("{id}/change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(int id, [FromBody] ChangePasswordDTO changePasswordDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound(); // Return 404 if the user is not found
            }

            // Verify the current password before allowing a password change
            if (!VerifyPassword(user.Password, changePasswordDto.CurrentPassword))
            {
                return BadRequest("Incorrect current password");
            }

            // Hash the new password before saving to the database
            user.Password = HashPassword(changePasswordDto.NewPassword);

            try
            {
                await _context.SaveChangesAsync();
                return Ok("Password changed successfully");
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500);
            }
        }

        // Verify the entered password against the stored hashed password
        private bool VerifyPassword(string storedHashedPassword, string enteredPassword)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(enteredPassword));
                var enteredHashedPassword = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();

                return storedHashedPassword == enteredHashedPassword;
            }
        }



        // Simple hashing function using SHA256
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
    }
}
