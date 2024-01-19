using AspNet.Security.OAuth.Validation;
using MarketPlace.Data;
using MarketPlace.Data.Entities;
using MarketPlace.Service;
using MarketPlace.Service.Interfaces;
using MarketPlace.Utilities.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(options =>
{
    options.AddPolicy("MyPolicy",
        builder => builder
        .SetIsOriginAllowed(isOriginAllowed: _ => true)
        .AllowAnyHeader()
        .AllowAnyMethod());
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("MyPolicy2",
        builder => builder
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod());
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddSwaggerGen(s =>
{
    s.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "MarketPlace API",
    });

    s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    s.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
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
    s.MapType(typeof(IFormFile), () => new OpenApiSchema() { Type = "file", Format = "binary" });
    s.OperationFilter<HeaderParameter>();
    //s.OperationFilter<SwaggerFileOperationFilter>();

});

var key = Encoding.ASCII.GetBytes(builder.Configuration.GetSection("Secret").Get<string>());
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateLifetime = true,
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero,
        //  RequireExpirationTime=true,
    };
});

builder.Services.AddDbContext<MarketPlaceDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("MarketPlaceConnection")));

builder.Services.AddIdentity<User, Role>(options =>
{
    //user password option
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.User.AllowedUserNameCharacters = null; // HERE!
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedAccount = false;

    options.Lockout.MaxFailedAccessAttempts = 3;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    //options.SignIn.RequireConfirmedEmail = true;
}).AddEntityFrameworkStores<MarketPlaceDbContext>().AddDefaultTokenProviders();

builder.Services.AddAuthentication(OAuthValidationDefaults.AuthenticationScheme);

builder.Services.AddScoped<IConcurrencyService, ConcurrencyService>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<UserManager<User>>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthorization();

app.UseAuthentication();

app.UseRouting();

app.UseDeveloperExceptionPage();

app.UseCors("MyPolicy");
app.UseCors("MyPolicy2");

app.UseAuthorization();

app.MapControllers();

app.Run();
