using System.ComponentModel.DataAnnotations;

namespace TrainingPlanner.Api.DTOs
{
    public class ExerciseCreateDto
    {
        [Required(ErrorMessage = "Name is required")]
        [MinLength(2, ErrorMessage = "Name must be at least 2 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "MuscleGroup is required")]
        public string MuscleGroup { get; set; } = string.Empty;
    }
}
