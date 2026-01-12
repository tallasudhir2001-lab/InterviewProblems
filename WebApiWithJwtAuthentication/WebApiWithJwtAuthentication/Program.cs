using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;

using WebApiWithJwtAuthentication.Data;
using WebApiWithJwtAuthentication.Models.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>//Registering Swagger
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme//Defines a security scheme named "Bearer".Swagger will later reference this name.
    {
        Name = "Authorization",//HTTP header name that Swagger will send.(see in devtools)
        Type = SecuritySchemeType.Http,//Tells Swagger this is HTTP authentication, not API key or OAuth.
        Scheme = "Bearer",//Specifies the authentication scheme.Maps to Authorization: Bearer <token> in devtools, Must be exactly "Bearer"
        BearerFormat = "JWT",
        In = ParameterLocation.Header,//Token goes in HTTP headers, not query string or body.
        Description = "Enter bearer token only without double quotes"//diplayed on the authorization textbox
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            /*“Apply the Bearer security scheme to all endpoints.”

Id = "Bearer" must match the name in AddSecurityDefinition
            */
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
//this must come before addauthentication for jwt, as this overrides authscehme in addauth to default(cookie based)
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();
//Enables authentication middleware, Sets JWT Bearer as default scheme
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
/*Registers JWT Bearer authentication handler.
 * This middleware:

Reads token from Authorization header

Validates it

Creates User identity
 * */
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,//Ensures token was issued by a trusted authority, Token must contain:iss = "MyApi" while creation , see in auth controller
        ValidateAudience = true,//Ensures token is intended for this API,Token must contain:  aud = "MyClient"
        ValidateLifetime = true,/*checks:

        exp(expiry)

nbf(not before)

Expired token → 401 Unauthorized
        */
        ValidateIssuerSigningKey = true,/*
          Ensures token signature is valid
 Prevents tampering

If token payload is modified → invalid signature.*/

        //Will need to read from appsettings.json rather than hardcoding
        ValidIssuer = builder.Configuration["Jwt:Issuer"],//"MyApi",
        ValidAudience = builder.Configuration["Jwt:Audience"],//"MyClient",
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])//("THIS_IS_A_VERY_SECRET_KEY_123456")
        ),
        RoleClaimType = ClaimTypes.Role
    };
});
builder.Services.AddAuthorization();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));



var app = builder.Build();
using(var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    string[] roles = { "Admin", "User","WeatherReporter" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
