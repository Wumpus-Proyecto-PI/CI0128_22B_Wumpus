using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PI.Areas.Identity.Data;
using PI.Data;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("UserDbApplicationContextConnection") ?? throw new InvalidOperationException("Connection string 'UserDbApplicationContextConnection' not found.");

builder.Services.AddDbContext<UserDbApplicationContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<UserApplication>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<UserDbApplicationContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddServerSideBlazor();
builder.Services.AddRazorPages();

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
app.UseAuthentication();;

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();
app.MapBlazorHub();

app.Run();
