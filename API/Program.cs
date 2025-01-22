using Microsoft.EntityFrameworkCore;
using Service.UserManagement;
using Service.UserManagement.Interface;
using EF.Models;
using Service.SystemSetup;
using Service.SystemSetup.Interface;
using Serilog;
using StackExchange.Redis;

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

#region UserManagement
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IModuleService, ModuleService>();
builder.Services.AddScoped<IDivisionService, DivisionService>();
builder.Services.AddScoped<IBureauService, BureauService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<ISectionService, SectionService>();
builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<IUserAccountService, UserAccountService>();
builder.Services.AddScoped<IPolicyService, PolicyService>();
#endregion

#region SystemSetup
builder.Services.AddScoped<IUnitOfMeasurementService, UnitOfMeasurementService>();
builder.Services.AddScoped<IPurchasingTypeService, PurchasingTypeService>();
builder.Services.AddScoped<IAccountCodeService, AccountCodeService>();
builder.Services.AddScoped<IMajorCategoryService, MajorCategoryService>();
builder.Services.AddScoped<ISubCategoryService, SubCategoryService>();
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<IPSDBMCatalogueService, PSDBMCatalogueService>();
builder.Services.AddScoped<ISupplementaryCatalogueService, SupplementaryCatalogueService>();
builder.Services.AddScoped<ISignatoryService, SignatoryService>();
builder.Services.AddScoped<IReferenceTableService, ReferenceTableService>();
builder.Services.AddScoped<IWorkFlowService, WorkFlowService>();
builder.Services.AddScoped<IWorkStepService, WorkStepService>();

#endregion

#endregion

#region Serilog

// Configure Serilog for daily rolling files
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Error()  // Log only errors
    //.WriteTo.Console() // Optional: Logs to the console
    .WriteTo.File(
        path: "Logs/log-.txt", // Log file name pattern
        rollingInterval: RollingInterval.Day, // Create a new file daily
        retainedFileCountLimit: 60, // Optional: Retain the last 60 days of logs
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
    )
    .CreateLogger();

// Use Serilog as the logging provider
builder.Host.UseSerilog();

#endregion

#region Redis
//builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
//{
//    var configuration = builder.Configuration.GetConnectionString("Redis");
//    return ConnectionMultiplexer.Connect(configuration);
//});

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
