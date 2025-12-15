using Microsoft.EntityFrameworkCore;
using TrainingPlanner.Api.Controllers;
using TrainingPlanner.Api.DTOs;
using TrainingPlanner.Api.Errors;
using TrainingPlanner.Domain.Entities;
using TrainingPlanner.Infrastructure.Data;

namespace TrainingPlanner.Api.Services
{
    public class ExerciseService
    {
        private readonly TrainingPlannerDbContext _context;
        private readonly ILogger<ExerciseService> _logger;

        public ExerciseService(TrainingPlannerDbContext context, ILogger<ExerciseService> logger)
        {
            _context = context;
            _logger = logger;
        }

        // CREATE
        public async Task<ExerciseDto> CreateExerciseAsync(ExerciseCreateDto dto)
        {
            // Enkel input-validering (400)
            if (dto == null)
                throw new ApiException(400, "Request body is required.");

            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ApiException(400, "Name is required.");

            if (string.IsNullOrWhiteSpace(dto.MuscleGroup))
                throw new ApiException(400, "MuscleGroup is required.");

            var exercise = new Exercise
            {
                Name = dto.Name.Trim(),
                MuscleGroup = dto.MuscleGroup.Trim()
            };

            _context.Exercises.Add(exercise);
            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "Created exercise {ExerciseId} ({Name}) targeting muscle group {MuscleGroup}",
                exercise.Id, exercise.Name, exercise.MuscleGroup);

            return new ExerciseDto
            {
                Id = exercise.Id,
                Name = exercise.Name,
                MuscleGroup = exercise.MuscleGroup
            };
        }

        // READ ALL
        public async Task<List<ExerciseDto>> GetAllExercisesAsync()
        {
            var exercises = await _context.Exercises
                .AsNoTracking()
                .Select(e => new ExerciseDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    MuscleGroup = e.MuscleGroup
                })
                .ToListAsync();

            _logger.LogInformation("Fetched {Count} exercises", exercises.Count);

            return exercises;
        }

        // READ ONE
        public async Task<ExerciseDto> GetExerciseByIdAsync(int id)
        {
            if (id <= 0)
                throw new ApiException(400, "Id must be greater than 0.");

            var exercise = await _context.Exercises
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);

            if (exercise == null)
            {
                _logger.LogWarning("Exercise {ExerciseId} was not found", id);
                throw new ApiException(404, "Exercise not found.");
            }

            return new ExerciseDto
            {
                Id = exercise.Id,
                Name = exercise.Name,
                MuscleGroup = exercise.MuscleGroup
            };
        }

        // UPDATE
        public async Task UpdateExerciseAsync(int id, ExerciseCreateDto dto)
        {
            if (id <= 0)
                throw new ApiException(400, "Id must be greater than 0.");

            if (dto == null)
                throw new ApiException(400, "Request body is required.");

            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ApiException(400, "Name is required.");

            if (string.IsNullOrWhiteSpace(dto.MuscleGroup))
                throw new ApiException(400, "MuscleGroup is required.");

            var exercise = await _context.Exercises.FindAsync(id);

            if (exercise == null)
            {
                _logger.LogWarning("Tried to update exercise {ExerciseId}, but it does not exist", id);
                throw new ApiException(404, "Exercise not found.");
            }

            exercise.Name = dto.Name.Trim();
            exercise.MuscleGroup = dto.MuscleGroup.Trim();

            await _context.SaveChangesAsync();

            _logger.LogInformation("Updated exercise {ExerciseId}", id);
        }

        // DELETE
        public async Task DeleteExerciseAsync(int id)
        {
            if (id <= 0)
                throw new ApiException(400, "Id must be greater than 0.");

            var exercise = await _context.Exercises.FindAsync(id);

            if (exercise == null)
            {
                _logger.LogWarning("Tried to delete exercise {ExerciseId}, but it does not exist", id);
                throw new ApiException(404, "Exercise not found.");
            }

            _context.Exercises.Remove(exercise);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Deleted exercise {ExerciseId}", id);
        }

    }
}
