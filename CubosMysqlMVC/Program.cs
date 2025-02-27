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

// REGISTRO DEL REPOSITORIO Y HELPER
builder.Services.AddScoped<RepositoryCubos>();
builder.Services.AddSingleton<HelperPathProvider>();

// REGISTRO DE CONTROLADORES
builder.Services.AddControllersWithViews();

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

app.Run();
