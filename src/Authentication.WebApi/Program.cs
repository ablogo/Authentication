using Amazon.SQS;
using Authentication.Core.Dtos;
using Authentication.Core.Entities;
using Authentication.Core.Interfaces;
using Authentication.DataAccess;
using Authentication.DataAccess.DbContext;
using Authentication.Infrastructure.ActionFilters;
using Authentication.Infrastructure.Jwt;
using Authentication.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configure log

builder.Host.ConfigureLogging(log =>
{
    log.ClearProviders();
    log.AddAWSProvider();
    log.AddConsole();
});

// Get configuration values

var _connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var _jwtSecret = builder.Configuration.GetValue<string>("Jwt:Secret");
var _jwtAudience = builder.Configuration.GetValue<string>("Jwt:Audience");
var _jwtIssuer = builder.Configuration.GetValue<string>("Jwt:Issuer");

builder.Services.Configure<AwsConfiguration>(builder.Configuration.GetSection("Aws"));
builder.Services.Configure<JwtConfiguration>(builder.Configuration.GetSection("Jwt"));

// Set db context

builder.Services.AddDbContext<AuthDbContext>(options => options.UseSqlServer(_connectionString));

//Add identity

builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.SignIn.RequireConfirmedEmail = true;
    options.SignIn.RequireConfirmedPhoneNumber = true;
    options.User.RequireUniqueEmail = true;
    options.Lockout.MaxFailedAccessAttempts = 5;
})
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddDefaultTokenProviders();

// Add Jwt validation

builder.Services.AddAuthentication(options =>
{
    // Identity made Cookie authentication the default.
    // However, we want JWT Bearer Auth to be the default.
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    //options.Authority = "https://localhost:8051";
    //options.Audience = "https://localhost:8051";
    //options.MetadataAddress = "https://localhost:8051";

    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidAlgorithms = new[] { SecurityAlgorithms.HmacSha512 },
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidAudience = _jwtAudience,
        ValidIssuer = _jwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret))
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(JwtBearerDefaults.AuthenticationScheme, policy =>
    {
        policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
        policy.RequireClaim(ClaimTypes.Name);
        policy.RequireClaim(ClaimTypes.Email);
        policy.RequireClaim(ClaimTypes.Role);
        policy.RequireClaim(ClaimTypes.NameIdentifier);
    });
});

// Add services to the container.

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IAccount, AccountService>();
builder.Services.AddScoped<IAuthentication, AuthenticationService>();
builder.Services.AddScoped<IAwsSqs, AwsSqsService>();
builder.Services.AddScoped<INotification, NotificationService>();
builder.Services.AddScoped<IRole, RoleService>();
builder.Services.AddScoped<IJwt, JwtManager>();

builder.Services.AddScoped<IRepository<User>, Repository<User>>();
builder.Services.AddScoped<IRepository<Account>, Repository<Account>>();

builder.Services.AddScoped<AccountActionFilter>();
builder.Services.AddScoped<UserActionFilter>();
builder.Services.AddScoped<EmailActionFilter>();
builder.Services.AddScoped<LoginActionFilter>();

// Add AWS services

builder.Services.AddAWSService<IAmazonSQS>();

//Normal configuration

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var securitySchema = new OpenApiSecurityScheme()
    {
        BearerFormat = "JWT",
        Description = "Jwt Authorization header use the Bearer schema.",
        Name = "Authorization",
        Scheme = "Bearer",
        Type = SecuritySchemeType.Http,
        Reference = new OpenApiReference()
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };

    var securityRequirement = new OpenApiSecurityRequirement()
    {
        { securitySchema, new string[] { "Bearer" } }
    };

    options.AddSecurityDefinition("Bearer", securitySchema);
    options.AddSecurityRequirement(securityRequirement);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
