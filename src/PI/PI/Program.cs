using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PI.Areas.Identity.Data;
using PI.Data;
using Microsoft.ApplicationInsights.AspNetCore;
using PI.EntityHandlers;
using PI.EntityModels;
using PI.Services;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("BaseDeDatos") ?? throw new InvalidOperationException("Connection string 'BaseDeDatos' not found.");

builder.Services.AddDbContext<UserDbApplicationContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDbContext<DataBaseContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<UserApplication>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<UserDbApplicationContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Configracion de requisitos de password
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 0;
});

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddServerSideBlazor();
builder.Services.AddRazorPages();
builder.Services.AddApplicationInsightsTelemetry();
builder.Services.AddScoped<DataBaseContext>();
builder.Services.AddScoped<UserDbApplicationContext>();
builder.Services.AddScoped<ProceduresServices>();
builder.Services.AddScoped<PI.EntityHandlers.AnalisisHandler>();
builder.Services.AddScoped<PI.EntityHandlers.NegocioHandler>();
builder.Services.AddScoped<PI.EntityHandlers.GastoFijoHandler>();
builder.Services.AddScoped<PI.EntityHandlers.ProductoHandler>();



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
