using Microsoft.EntityFrameworkCore;
using Store.DataAccess.Data;
using Store.DataAccess.Implementation;
using Store.Entities.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
// Add Razor Runtime Compilation Service to Compile while i write in project 
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
// add dbContext 
// add to your app context
builder.Services.AddDbContext<ApplicationDbContext>(options=>options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection") //=> U can add it as string
                                                                   // like that: "Server=.;Database=Electronics_Store;Trusted_Connection=True;Integrated Security=True;MultipleActiveResultSets=True;"
    ));


// Inject UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

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
    pattern: "{area=exists}/{controller=Home}/{action=Index}/{id?}");

app.Run();
