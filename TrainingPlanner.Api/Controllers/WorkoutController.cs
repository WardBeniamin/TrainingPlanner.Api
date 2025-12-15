using Microsoft.AspNetCore.Mvc;
using TrainingPlanner.Api.DTOs;
using TrainingPlanner.Api.Services;

namespace TrainingPlanner.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkoutController : ControllerBase
    {
        private readonly WorkoutService _workoutService;

        public WorkoutController(WorkoutService workoutService)
        {
            _workoutService = workoutService;
        }

        // POST: api/workout
        [HttpPost]
        public async Task<ActionResult<WorkoutDto>> Create([FromBody] WorkoutCreateDto dto)
        {
            var result = await _workoutService.CreateWorkoutAsync(dto);
            return CreatedAtAction(nameof(GetOne), new { id = result.Id }, result);
        }

        // GET: api/workout
        [HttpGet]
        public async Task<ActionResult<List<WorkoutDto>>> GetAll()
        {
            var result = await _workoutService.GetAllWorkoutsAsync();
            return Ok(result);
        }

        // GET: api/workout/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<WorkoutDto>> GetOne(int id)
        {
            var workout = await _workoutService.GetWorkoutByIdAsync(id);
            return Ok(workout);
        }

        // PUT: api/workout/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] WorkoutCreateDto dto)
        {
            await _workoutService.UpdateWorkoutAsync(id, dto);
            return NoContent();
        }

        // DELETE: api/workout/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _workoutService.DeleteWorkoutAsync(id);
            return NoContent();
        }

        // POST: api/workout/{workoutId}/exercise
        [HttpPost("{workoutId:int}/exercise")]
        public async Task<IActionResult> AddExercise(int workoutId, [FromBody] AddExerciseToWorkoutDto dto)
        {
            await _workoutService.AddExerciseToWorkoutAsync(workoutId, dto);
            return NoContent();
        }
    }
}
