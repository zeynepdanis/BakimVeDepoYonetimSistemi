using BakimVeDepoYonetimSistemi.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<RepositoryContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


  builder.Services.AddAuthentication(options =>
   {
       options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
       options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
   })
    .AddCookie()
   .AddGoogle(options =>
   {
       options.ClientId = "808698319405-nkt4d5jm9e38chefqj96uajv9tib629l.apps.googleusercontent.com";
       options.ClientSecret = "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";
       options.CallbackPath = "/signin-google";



   });

builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<TeamMemberRepository>();
builder.Services.AddScoped<MaintenanceRepository>();
builder.Services.AddScoped<TeamRepository>();
builder.Services.AddAuthorization();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

app.UseAuthentication();

app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



app.MapControllers();

app.Run();
