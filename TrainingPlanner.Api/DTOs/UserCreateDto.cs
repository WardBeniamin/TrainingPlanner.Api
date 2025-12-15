using System.ComponentModel.DataAnnotations;

namespace TrainingPlanner.Api.DTOs
{
    // DTO som används när vi skapar eller uppdaterar en användare
    public class UserCreateDto
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name can be max 100 characters")]
        public string Name { get; set; } = "";

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Email must be a valid email address")]
        [StringLength(200, ErrorMessage = "Email can be max 200 characters")]
        public string Email { get; set; } = "";
    }
}
