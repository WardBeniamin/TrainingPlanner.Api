namespace TrainingPlanner.Domain.Entities
{
    // Join-tabell mellan Workout och Exercise
    public class WorkoutExercise
    {
        public int WorkoutId { get; set; }       // FK till Workout
        public Workout? Workout { get; set; }

        public int ExerciseId { get; set; }      // FK till Exercise
        public Exercise? Exercise { get; set; }

        // Extra info (valfritt)
        public int Sets { get; set; }            // Antal set
        public int Reps { get; set; }            // Antal reps
    }
}
