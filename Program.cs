global using web.Models;
global using web.Services.CharacterService;
global using web.Dtos.Character;
global using AutoMapper;
global using Microsoft.EntityFrameworkCore;
global using web.Data;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => 
{
    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme {
        Description = "Standard Authorization header using bearer schema, e.g \"bearer {token} \"",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.OperationFilter<SecurityRequirementsOperationFilter>();
}

);

// register CharacterService class as a implementaiton of ICharacterService
builder.Services.AddScoped<ICharacterService,CharacterService>();
// register AuthRepository class as a implentation of IAuthRepository
builder.Services.AddScoped<IAuthRepository,AuthRepository>();

builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddDbContext<DataContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// add auth scheme to authorize

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options => {  
    options.TokenValidationParameters = new TokenValidationParameters{
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8
        .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
        ValidateIssuer = false, 
        ValidateAudience = false
    };
});

var app = builder.Build();

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
