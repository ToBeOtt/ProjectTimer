using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProjectTimer.Data;
using ProjectTimer.Services.Clocks;
using ProjectTimer.Services.Projects;
using ProjectTimer.Services.Sessions;
using System;
using Microsoft.AspNetCore.Identity;
using ProjectTimer.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using ProjectTimer.Services.Users;
using ProjectTimer.Services.SavedProjectTimes;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

//builder.Services.AddAuthorization(options =>
//{
//    options.FallbackPolicy = new AuthorizationPolicyBuilder()
//        .RequireAuthenticatedUser()
//        .Build();
//});

builder.Services.AddScoped<ProjectService>();
builder.Services.AddScoped<ClockService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<SavedProjectTimeService>();
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<DataContext>();
builder.Services.AddDistributedMemoryCache(); // Skapar m�jlighet att lagra session i cookie
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(5); //St�nger av session om man inte gjort n�got p� 5 timmar.
}
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession(); // Ska vara mellan UseRouting och MapRazorPages f�r att fungera korrekt.

app.MapRazorPages();

app.Run();
