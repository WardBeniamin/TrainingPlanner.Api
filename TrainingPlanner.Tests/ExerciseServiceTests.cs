using Microsoft.Extensions.Logging.Abstractions;
using TrainingPlanner.Api.DTOs;
using TrainingPlanner.Api.Services;
using Xunit;
using TrainingPlanner.Api.Errors;

namespace TrainingPlanner.Tests;

public class ExerciseServiceTests
{
    [Fact]
    public async Task CreateExerciseAsync_ShouldCreateExercise()
    {
        // Arrange (förbered)
        var context = TestDbFactory.CreateDbContext();
        var logger = NullLogger<ExerciseService>.Instance;
        var service = new ExerciseService(context, logger);

        var dto = new ExerciseCreateDto
        {
            Name = "Bench Press",
            MuscleGroup = "Chest"
        };

        // Act (gör)
        var result = await service.CreateExerciseAsync(dto);

        // Assert (kolla)
        Assert.True(result.Id > 0);
        Assert.Equal("Bench Press", result.Name);
        Assert.Equal("Chest", result.MuscleGroup);
    }

    [Fact]
    public async Task GetAllExercisesAsync_ShouldReturnAllExercises()
    {
        // Arrange
        var context = TestDbFactory.CreateDbContext();
        var logger = NullLogger<ExerciseService>.Instance;
        var service = new ExerciseService(context, logger);

        await service.CreateExerciseAsync(new ExerciseCreateDto { Name = "Squat", MuscleGroup = "Legs" });
        await service.CreateExerciseAsync(new ExerciseCreateDto { Name = "Pull-up", MuscleGroup = "Back" });

        // Act
        var all = await service.GetAllExercisesAsync();

        // Assert
        Assert.Equal(2, all.Count);
    }

    [Fact]
    public async Task GetExerciseByIdAsync_ShouldReturnExercise_WhenExists()
    {
        // Arrange
        var context = TestDbFactory.CreateDbContext();
        var logger = NullLogger<ExerciseService>.Instance;
        var service = new ExerciseService(context, logger);

        var created = await service.CreateExerciseAsync(new ExerciseCreateDto { Name = "Deadlift", MuscleGroup = "Back" });

        // Act
        var one = await service.GetExerciseByIdAsync(created.Id);

        // Assert
        Assert.Equal(created.Id, one.Id);
        Assert.Equal("Deadlift", one.Name);
    }



    [Fact]
    public async Task UpdateExerciseAsync_ShouldUpdateExistingExercise()
    {
        // Arrange
        var context = TestDbFactory.CreateDbContext();
        var logger = NullLogger<ExerciseService>.Instance;
        var service = new ExerciseService(context, logger);

        var created = await service.CreateExerciseAsync(new ExerciseCreateDto
        {
            Name = "Old Name",
            MuscleGroup = "Old Group"
        });

        var updateDto = new ExerciseCreateDto
        {
            Name = "New Name",
            MuscleGroup = "New Group"
        };

        // Act
        await service.UpdateExerciseAsync(created.Id, updateDto);

        // Assert
        var updated = await service.GetExerciseByIdAsync(created.Id);
        Assert.Equal("New Name", updated.Name);
        Assert.Equal("New Group", updated.MuscleGroup);
    }

    [Fact]
    public async Task DeleteExerciseAsync_ShouldRemoveExercise()
    {
        // Arrange
        var context = TestDbFactory.CreateDbContext();
        var logger = NullLogger<ExerciseService>.Instance;
        var service = new ExerciseService(context, logger);

        var created = await service.CreateExerciseAsync(new ExerciseCreateDto
        {
            Name = "To Delete",
            MuscleGroup = "Chest"
        });

        // Act
        await service.DeleteExerciseAsync(created.Id);

        // Assert (ska kasta 404 eftersom den inte finns längre)
        await Assert.ThrowsAsync<ApiException>(() => service.GetExerciseByIdAsync(created.Id));
    }

    [Fact]
    public async Task CreateExerciseAsync_ShouldThrow_WhenNameIsMissing()
    {
        // Arrange
        var context = TestDbFactory.CreateDbContext();
        var logger = NullLogger<ExerciseService>.Instance;
        var service = new ExerciseService(context, logger);

        var dto = new ExerciseCreateDto
        {
            Name = "",
            MuscleGroup = "Back"
        };

        // Act + Assert
        var ex = await Assert.ThrowsAsync<ApiException>(() => service.CreateExerciseAsync(dto));
        Assert.Equal(400, ex.StatusCode);
    }

    [Fact]
    public async Task GetExerciseByIdAsync_ShouldThrow404_WhenNotFound()
    {
        // Arrange
        var context = TestDbFactory.CreateDbContext();
        var logger = NullLogger<ExerciseService>.Instance;
        var service = new ExerciseService(context, logger);

        // Act + Assert
        var ex = await Assert.ThrowsAsync<ApiException>(() => service.GetExerciseByIdAsync(9999));
        Assert.Equal(404, ex.StatusCode);
    }

    [Fact]
    public async Task DeleteExerciseAsync_ShouldThrow404_WhenNotFound()
    {
        // Arrange
        var context = TestDbFactory.CreateDbContext();
        var logger = NullLogger<ExerciseService>.Instance;
        var service = new ExerciseService(context, logger);

        // Act + Assert
        var ex = await Assert.ThrowsAsync<ApiException>(() => service.DeleteExerciseAsync(9999));
        Assert.Equal(404, ex.StatusCode);
    }

}

