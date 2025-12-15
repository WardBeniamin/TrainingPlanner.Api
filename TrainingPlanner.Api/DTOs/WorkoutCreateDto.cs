using System.ComponentModel.DataAnnotations;

namespace TrainingPlanner.Api.DTOs
{
    public class WorkoutCreateDto
    {
        [Required(ErrorMessage = "Title is required")]
        [MinLength(2, ErrorMessage = "Title must be at least 2 characters")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date is required")]
        public DateTime Date { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "UserId must be greater than 0")]
        public int UserId { get; set; }
    }
}
