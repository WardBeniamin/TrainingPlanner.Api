using Microsoft.EntityFrameworkCore;
using TrainingPlanner.Domain.Entities;

namespace TrainingPlanner.Infrastructure.Data
{
    // DbContext är "dörren" mellan C#-världen och databasen
    public class TrainingPlannerDbContext : DbContext
    {
        public TrainingPlannerDbContext(DbContextOptions<TrainingPlannerDbContext> options)
            : base(options)
        {
        }

        // Tabeller
        public DbSet<User> Users { get; set; }
        public DbSet<Workout> Workouts { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<WorkoutExercise> WorkoutExercises { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Many-to-many med join-entity WorkoutExercise
            modelBuilder.Entity<WorkoutExercise>()
                .HasKey(we => new { we.WorkoutId, we.ExerciseId });

            modelBuilder.Entity<WorkoutExercise>()
                .HasOne(we => we.Workout)
                .WithMany(w => w.WorkoutExercises)
                .HasForeignKey(we => we.WorkoutId);

            modelBuilder.Entity<WorkoutExercise>()
                .HasOne(we => we.Exercise)
                .WithMany(e => e.WorkoutExercises)
                .HasForeignKey(we => we.ExerciseId);
        }
    }
}
