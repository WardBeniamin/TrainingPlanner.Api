using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TrainingPlanner.Api.DTOs;
using TrainingPlanner.Domain.Entities;
using TrainingPlanner.Infrastructure.Data;

namespace TrainingPlanner.Api.Services
{
    public class UserService
    {
        private readonly TrainingPlannerDbContext _context;
        private readonly ILogger<UserService> _logger;

        public UserService(TrainingPlannerDbContext context, ILogger<UserService> logger)
        {
            _context = context;
            _logger = logger;
        }

        // CREATE
        public async Task<UserDto> CreateUserAsync(UserCreateDto dto)
        {
            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // enkel loggning
            _logger.LogInformation("Created user with Id {UserId} and Email {Email}", user.Id, user.Email);

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            };
        }

        // READ ALL
        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var users = await _context.Users
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email
                })
                .ToListAsync();

            _logger.LogInformation("Fetched {Count} users", users.Count);

            return users;
        }

        // READ ONE
        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                _logger.LogWarning("Tried to get user with Id {UserId}, but it does not exist", id);
                return null;
            }

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            };
        }

        // UPDATE
        public async Task<bool> UpdateUserAsync(int id, UserCreateDto dto)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                _logger.LogWarning("Tried to update user with Id {UserId}, but it does not exist", id);
                return false;
            }

            user.Name = dto.Name;
            user.Email = dto.Email;

            await _context.SaveChangesAsync();
            _logger.LogInformation("Updated user with Id {UserId}", id);

            return true;
        }

        // DELETE
        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                _logger.LogWarning("Tried to delete user with Id {UserId}, but it does not exist", id);
                return false;
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Deleted user with Id {UserId}", id);

            return true;
        }
    }
}
