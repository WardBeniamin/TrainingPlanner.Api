namespace TrainingPlanner.Domain.Entities
{
    // Ett träningspass, t.ex. "Måndag - Bröst & Triceps"
    public class Workout
    {
        public int Id { get; set; }               // Primärnyckel
        public string Title { get; set; } = "";   // Titel på passet
        public DateTime Date { get; set; }        // Datum för passet

        // Koppling till User (one-to-many)
        public int UserId { get; set; }           // FK till User
        public User? User { get; set; }           // Navigation

        // Many-to-many med Exercise via join-tabell
        public ICollection<WorkoutExercise> WorkoutExercises { get; set; } = new List<WorkoutExercise>();
    }
}
