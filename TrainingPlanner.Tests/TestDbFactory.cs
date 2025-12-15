using Microsoft.EntityFrameworkCore;
using TrainingPlanner.Infrastructure.Data;

namespace TrainingPlanner.Tests;

public static class TestDbFactory
{
    public static TrainingPlannerDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<TrainingPlannerDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // unik DB per test
            .Options;

        return new TrainingPlannerDbContext(options);
    }
}
