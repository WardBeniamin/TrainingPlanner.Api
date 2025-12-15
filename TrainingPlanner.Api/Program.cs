using Microsoft.EntityFrameworkCore;
using TrainingPlanner.Infrastructure.Data;
using TrainingPlanner.Api.Services;
using TrainingPlanner.Api.Errors;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// DbContext
builder.Services.AddDbContext<TrainingPlannerDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TrainingPlannerConnection"))
);

// Services
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<WorkoutService>();
builder.Services.AddScoped<ExerciseService>();

// Controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger (dev)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Global error handling (ska ligga tidigt i pipeline)
app.UseMiddleware<ExceptionMiddleware>();
app.UseCors("AllowFrontend");
app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

app.Run();
