namespace TrainingPlanner.Api.DTOs
{
    // Det vi skickar tillbaka till klienten
    public class WorkoutDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public DateTime Date { get; set; }
        public int UserId { get; set; }
    }
}
