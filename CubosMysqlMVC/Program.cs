using PracticaCubosMVC.Data;
using PracticaCubosMVC.Repositories;
using PracticaCubosMVC.Helpers;
using Microsoft.EntityFrameworkCore;
using MySql.EntityFrameworkCore.Extensions;

var builder = WebApplication.CreateBuilder(args);

// REGISTRO DEL DbContext USANDO MySQL
builder.Services.AddDbContext<CubosContext>(options =>
    options.UseMySQL(builder.Configuration.GetConnectionString("MySqlConnection"))
);

// REGISTRO DEL REPOSITORIO
builder.Services.AddScoped<RepositoryCubos>();

// REGISTRO DE CONTROLADORES
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

//HELPERS
builder.Services.AddSingleton<HelperPathProvider>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<HelperSession>();
builder.Services.AddSingleton<HelperCache>();


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseSession();

app.Run();
