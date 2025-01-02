using EMGMSA.Seeder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);


//configuration du context de la base de donnees SQL SERVER

builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer("Server=localhost; Database=EMGMSA; User ID=sa; Password=Kerim7777!; MultipleActiveResultSets=true; TrustServerCertificate=True;"));

//ajouter les services pour ASP.NET Core Identity

builder.Services.AddIdentity<User, IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();



//on doit enregistrer le service CarSeeder dans le conteneur dinjection de
//dependance pour quil soit disponible dans les controleurs
//on a pas eu a faire cet enregistrements pr model et controller pck 
//ASP.NET Core le prend en charge automatiquement grace a la convention MVC 
builder.Services.AddScoped<CarSeeder>();
var app = builder.Build();


//MiddleWare pour gerer lauthentification et lautorisation

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Cars}/{action=Index}/{id?}");

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

app.MapRazorPages();

app.Run();
