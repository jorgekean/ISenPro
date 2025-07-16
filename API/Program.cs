using Microsoft.EntityFrameworkCore;
using Service.UserManagement;
using Service.UserManagement.Interface;
using EF.Models;
using Service.SystemSetup;
using Service.SystemSetup.Interface;
using Serilog;
using StackExchange.Redis;
using Service.Transaction.Interface;
using Service.Transaction;
using Service.Cache;
using Service;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Service.Service;

var builder = WebApplication.CreateBuilder(args);

// Add environment-specific configuration
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

var allowedOrigins = builder.Configuration.GetSection("CorsSettings:AllowedOrigins").Get<string[]>();

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins(allowedOrigins ?? ["http://localhost:3000"])
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});

#region JWT Auth
var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // Use true in production with HTTPS
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ClockSkew = TimeSpan.Zero
    };
});

// Optionally, configure role-based policies.
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    // Add additional policies if needed.
});
#endregion

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region EF
var connectionString = builder.Configuration.GetConnectionString("Default");

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException($"Connection string for environment '{builder.Environment.EnvironmentName}' is not configured.");
}


// Register the connection string for use in DI
builder.Services.AddSingleton(sp => new { ConnectionString = connectionString });

// Add DbContext to the DI container
builder.Services.AddDbContext<ISenProContext>(options =>
    options.UseSqlServer(connectionString));
#endregion

#region User
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserContext, UserContext>();
#endregion

#region MemoryCache
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<IGenericCacheService, GenericCacheService>();

builder.Services.AddScoped<CachedItems>();
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

#region Transaction
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IAppService, AppService>();
builder.Services.AddScoped<IPpmpService, PpmpService>();
builder.Services.AddScoped<IPrService, PrService>();
#endregion

#region Authentication
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();

#endregion

#endregion

#region Serilog
// Read configuration
var logFilePath = builder.Configuration["Logging:Serilog:LogFilePath"];

// Configure Serilog for daily rolling files
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Error()  // Log only errors
                           //.WriteTo.Console() // Optional: Logs to the console
    .WriteTo.File(
        path: logFilePath ?? "Logs/log-.txt", // Log file name pattern
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

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
