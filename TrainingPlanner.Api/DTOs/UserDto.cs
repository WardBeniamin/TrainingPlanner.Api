namespace TrainingPlanner.Api.DTOs
{
    // DTO som skickas ut från API:t
    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
    }
}
