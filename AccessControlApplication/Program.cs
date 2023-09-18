using AccessControlApplication.Data;
using AccessControlApplication.Email;
using AccessControlApplication.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<IEmailSender, EmailSender>();

builder.Services.AddTransient<ICombinedClasses, CombinedClasses>();

builder.Services.AddTransient<ILoggedUser, LoggedUser>();

builder.Services.AddTransient<IRegister, Register>();

builder.Services.AddTransient<ISearchByCategory, SearchByCategory>();

builder.Services.AddTransient<IButtonControls, ButtonControls>();

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(Options =>
                        Options.UseSqlServer(builder.Configuration.GetConnectionString("Connection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
