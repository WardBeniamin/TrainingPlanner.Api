namespace TrainingPlanner.Api.DTOs
{
    // Det vi skickar tillbaka till klienten
    public class ExerciseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string MuscleGroup { get; set; } = "";
    }
}
