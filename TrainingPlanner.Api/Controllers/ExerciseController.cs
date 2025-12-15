using Microsoft.AspNetCore.Mvc;
using TrainingPlanner.Api.DTOs;
using TrainingPlanner.Api.Services;

namespace TrainingPlanner.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExerciseController : ControllerBase
    {
        private readonly ExerciseService _exerciseService;

        public ExerciseController(ExerciseService exerciseService)
        {
            _exerciseService = exerciseService;
        }

        // POST: api/exercise
        [HttpPost]
        public async Task<ActionResult<ExerciseDto>> Create([FromBody] ExerciseCreateDto dto)
        {
            var result = await _exerciseService.CreateExerciseAsync(dto);

            // 201 Created är "rätt" för Create
            return CreatedAtAction(nameof(GetOne), new { id = result.Id }, result);
        }

        // GET: api/exercise
        [HttpGet]
        public async Task<ActionResult<List<ExerciseDto>>> GetAll()
        {
            var result = await _exerciseService.GetAllExercisesAsync();
            return Ok(result);
        }

        // GET: api/exercise/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ExerciseDto>> GetOne(int id)
        {
            // Om den inte finns -> service kastar ApiException(404) -> middleware fixar
            var exercise = await _exerciseService.GetExerciseByIdAsync(id);
            return Ok(exercise);
        }

        // PUT: api/exercise/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] ExerciseCreateDto dto)
        {
            // Om den inte finns -> ApiException(404)
            // Om dto är fel -> ApiException(400)
            await _exerciseService.UpdateExerciseAsync(id, dto);
            return NoContent();
        }

        // DELETE: api/exercise/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            // Om den inte finns -> ApiException(404)
            await _exerciseService.DeleteExerciseAsync(id);
            return NoContent();
        }
    }
}
