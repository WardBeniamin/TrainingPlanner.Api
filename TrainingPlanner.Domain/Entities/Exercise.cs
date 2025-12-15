namespace TrainingPlanner.Domain.Entities
{
    // En övning, t.ex. Bänkpress
    public class Exercise
    {
        public int Id { get; set; }                 // Primärnyckel
        public string Name { get; set; } = "";      // Namn på övningen
        public string MuscleGroup { get; set; } = ""; // Muskelgrupp (valfritt fält)

        // Navigation: vilka workouts använder denna övning
        public ICollection<WorkoutExercise> WorkoutExercises { get; set; } = new List<WorkoutExercise>();
    }
}
