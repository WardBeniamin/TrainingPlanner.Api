using System.ComponentModel.DataAnnotations;

namespace TrainingPlanner.Api.DTOs
{
    public class AddExerciseToWorkoutDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "ExerciseId must be greater than 0")]
        public int ExerciseId { get; set; }

        [Range(1, 50, ErrorMessage = "Sets must be between 1 and 50")]
        public int Sets { get; set; }

        [Range(1, 200, ErrorMessage = "Reps must be between 1 and 200")]
        public int Reps { get; set; }
    }
}
