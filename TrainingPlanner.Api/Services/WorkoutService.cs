using Microsoft.EntityFrameworkCore;
using TrainingPlanner.Api.DTOs;
using TrainingPlanner.Api.Errors;
using TrainingPlanner.Domain.Entities;
using TrainingPlanner.Infrastructure.Data;

namespace TrainingPlanner.Api.Services
{
    public class WorkoutService
    {
        private readonly TrainingPlannerDbContext _context;
        private readonly ILogger<WorkoutService> _logger;

        public WorkoutService(TrainingPlannerDbContext context, ILogger<WorkoutService> logger)
        {
            _context = context;
            _logger = logger;
        }

        // CREATE
        public async Task<WorkoutDto> CreateWorkoutAsync(WorkoutCreateDto dto)
        {
            if (dto == null)
                throw new ApiException(400, "Request body is required.");

            if (string.IsNullOrWhiteSpace(dto.Title))
                throw new ApiException(400, "Title is required.");

            if (dto.UserId <= 0)
                throw new ApiException(400, "UserId must be greater than 0.");

            var userExists = await _context.Users.AnyAsync(u => u.Id == dto.UserId);
            if (!userExists)
                throw new ApiException(404, "User does not exist.");

            var workout = new Workout
            {
                Title = dto.Title.Trim(),
                Date = dto.Date,
                UserId = dto.UserId
            };

            _context.Workouts.Add(workout);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Created workout {WorkoutId} for User {UserId}", workout.Id, workout.UserId);

            return new WorkoutDto
            {
                Id = workout.Id,
                Title = workout.Title,
                Date = workout.Date,
                UserId = workout.UserId
            };
        }

        // READ ALL
        public async Task<List<WorkoutDto>> GetAllWorkoutsAsync()
        {
            return await _context.Workouts
                .AsNoTracking()
                .Select(w => new WorkoutDto
                {
                    Id = w.Id,
                    Title = w.Title,
                    Date = w.Date,
                    UserId = w.UserId
                })
                .ToListAsync();
        }

        // READ ONE
        public async Task<WorkoutDto> GetWorkoutByIdAsync(int id)
        {
            if (id <= 0)
                throw new ApiException(400, "Id must be greater than 0.");

            var workout = await _context.Workouts
                .AsNoTracking()
                .FirstOrDefaultAsync(w => w.Id == id);

            if (workout == null)
                throw new ApiException(404, "Workout not found.");

            return new WorkoutDto
            {
                Id = workout.Id,
                Title = workout.Title,
                Date = workout.Date,
                UserId = workout.UserId
            };
        }

        // UPDATE
        public async Task UpdateWorkoutAsync(int id, WorkoutCreateDto dto)
        {
            if (id <= 0)
                throw new ApiException(400, "Id must be greater than 0.");

            if (dto == null)
                throw new ApiException(400, "Request body is required.");

            if (string.IsNullOrWhiteSpace(dto.Title))
                throw new ApiException(400, "Title is required.");

            if (dto.UserId <= 0)
                throw new ApiException(400, "UserId must be greater than 0.");

            var workout = await _context.Workouts.FindAsync(id);
            if (workout == null)
                throw new ApiException(404, "Workout not found.");

            var userExists = await _context.Users.AnyAsync(u => u.Id == dto.UserId);
            if (!userExists)
                throw new ApiException(404, "User does not exist.");

            workout.Title = dto.Title.Trim();
            workout.Date = dto.Date;
            workout.UserId = dto.UserId;

            await _context.SaveChangesAsync();
            _logger.LogInformation("Updated workout {WorkoutId}", id);
        }

        // DELETE
        public async Task DeleteWorkoutAsync(int id)
        {
            if (id <= 0)
                throw new ApiException(400, "Id must be greater than 0.");

            var workout = await _context.Workouts.FindAsync(id);
            if (workout == null)
                throw new ApiException(404, "Workout not found.");

            _context.Workouts.Remove(workout);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Deleted workout {WorkoutId}", id);
        }

        // MANY-TO-MANY: Add/Update Exercise on Workout
        public async Task AddExerciseToWorkoutAsync(int workoutId, AddExerciseToWorkoutDto dto)
        {
            if (workoutId <= 0)
                throw new ApiException(400, "workoutId must be greater than 0.");

            if (dto == null)
                throw new ApiException(400, "Request body is required.");

            if (dto.ExerciseId <= 0)
                throw new ApiException(400, "ExerciseId must be greater than 0.");

            if (dto.Sets <= 0)
                throw new ApiException(400, "Sets must be greater than 0.");

            if (dto.Reps <= 0)
                throw new ApiException(400, "Reps must be greater than 0.");

            var workoutExists = await _context.Workouts.AnyAsync(w => w.Id == workoutId);
            if (!workoutExists)
                throw new ApiException(404, "Workout not found.");

            var exerciseExists = await _context.Exercises.AnyAsync(e => e.Id == dto.ExerciseId);
            if (!exerciseExists)
                throw new ApiException(404, "Exercise does not exist.");

            // PK(workoutId, exerciseId) antas i WorkoutExercises
            var existing = await _context.WorkoutExercises.FindAsync(workoutId, dto.ExerciseId);

            if (existing != null)
            {
                existing.Sets = dto.Sets;
                existing.Reps = dto.Reps;
                _logger.LogInformation("Updated WorkoutExercise: Workout {WorkoutId}, Exercise {ExerciseId}", workoutId, dto.ExerciseId);
            }
            else
            {
                var we = new WorkoutExercise
                {
                    WorkoutId = workoutId,
                    ExerciseId = dto.ExerciseId,
                    Sets = dto.Sets,
                    Reps = dto.Reps
                };

                _context.WorkoutExercises.Add(we);
                _logger.LogInformation("Added WorkoutExercise: Workout {WorkoutId}, Exercise {ExerciseId}", workoutId, dto.ExerciseId);
            }

            await _context.SaveChangesAsync();
        }
    }
}
