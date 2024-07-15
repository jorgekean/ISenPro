using Microsoft.EntityFrameworkCore;
using Repository.Models;
using Service.UserManagement;
using Service.UserManagement.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("http://localhost:3000")
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region EF
// Add DbContext to the DI container
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       "Server=(LocalDB)\\MSSQLLocalDB;Database=TestinganDB;Integrated Security=True;";

builder.Services.AddDbContext<ISenProContext>(options =>
    options.UseSqlServer(connectionString));
#endregion

#region Services

builder.Services.AddScoped<IRoleService, RoleService>();
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable CORS
app.UseCors("AllowSpecificOrigin");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
