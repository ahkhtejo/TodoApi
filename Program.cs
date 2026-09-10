using System.Text.Json.Serialization;
using Scalar.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Serilog;


var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter());
    }); ;

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});



builder.Services.AddTransient<UserService>();
builder.Services.AddTransient<TaskService>();
builder.Services.AddTransient<StatusSarvice>();
builder.Services.AddTransient<CommentsService>();





var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
   {
       options.Title = "ToDo App";
       options.Theme = ScalarTheme.BluePlanet;

   });
}



app.UseHttpsRedirection();

app.MapControllers();

app.Run();

