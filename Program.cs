using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProjectTimer.Data;
using ProjectTimer.Services.Projects;
using ProjectTimer.Services.ProjectSessionTimers;
using ProjectTimer.Services.Sessions;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddScoped<ProjectService>();
builder.Services.AddScoped<SessionService>();
builder.Services.AddScoped<ProjectSessionTimerService>();
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddDistributedMemoryCache(); // Skapar möjlighet att lagra session i cookie
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(1); //Stänger av session om man inte gjort något på 5 timmar.
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

app.UseSession(); // Ska vara mellan UseRouting och MapRazorPages för att fungera korrekt.

app.MapRazorPages();

app.Run();
