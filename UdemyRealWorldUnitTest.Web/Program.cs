using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using UdemyRealWorldUnitTest.Web.Models;
using UdemyRealWorldUnitTest.Web.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<UdemyUnitTestDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Add services to the container.
builder.Services.AddControllersWithViews();

//In-Memory Cache yapabilmek için ekleyelim
builder.Services.AddMemoryCache();


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
