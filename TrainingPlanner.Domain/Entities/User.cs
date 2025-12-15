namespace TrainingPlanner.Domain.Entities
{
    // En användare i systemet
    public class User
    {
        public int Id { get; set; }            // Primärnyckel
        public string Name { get; set; } = ""; // Namn på användaren
        public string Email { get; set; } = ""; // E-post till användaren

        // Navigation: en user kan ha många workouts
        public ICollection<Workout> Workouts { get; set; } = new List<Workout>();
    }
}
