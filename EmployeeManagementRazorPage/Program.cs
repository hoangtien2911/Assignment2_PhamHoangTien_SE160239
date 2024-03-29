using EmployeeManagementBO.Models;
using EmployeeManagementService;
using EmployeeManagementService.IService;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IJobService, JobService>();
builder.Services.AddScoped<IJobHistoryService, JobHistoryService>();
// Add services to the container.
builder.Services.AddRazorPages();

// Add logging configuration
builder.Logging.AddConsole(); // Log to console
builder.Logging.AddDebug(); // Log to debug output

// Configure logging to log SQL queries
builder.Services.AddDbContext<EmployeesManagementContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("EmployeeManagement"));
    options.LogTo(Console.WriteLine); // Log SQL queries to console
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.AccessDeniedPath = "/AccountPermission/AccessDenied"; // Set the access denied page path
});
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();