using EMGMSA.Seeder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Options;


var builder = WebApplication.CreateBuilder(args);

var connectionString =Environment.GetEnvironmentVariable("DATABASE_URL")?? builder.Configuration.GetConnectionString("DefaultConnection") ;


builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseNpgsql(connectionString));
;
//configuration du context de la base de donnees SQL SERVER

// builder.Services.AddDbContext<ApplicationDbContext>(options =>
// options.UseSqlServer("Server=localhost; Database=EMGMSA; User ID=sa; Password=Kerim7777!; MultipleActiveResultSets=true; TrustServerCertificate=True;"));

//ajouter les services pour ASP.NET Core Identity

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();


// builder.Services.ConfigureApplicationCookie(options =>
// {
//     options.Cookie.HttpOnly = true;
//     options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
//     options.LoginPath = "/Account/Login";
//     options.LogoutPath = "/Account/Logout";
//     options.SlidingExpiration = true;
// });

//var jwtSettings = builder.Configuration.GetSection("JwtSettings");


// builder.Services.AddAuthentication(options =>
// {
//     options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//     options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
// })
// .AddJwtBearer(options =>
// {
//     options.TokenValidationParameters = new TokenValidationParameters
//     {
//         ValidateIssuer = true,
//         ValidateAudience = true,
//         ValidateLifetime = true,
//         ValidateIssuerSigningKey = true,
//         ValidIssuer = jwtSettings["Issuer"],
//         ValidAudience = jwtSettings["Audience"],
//         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]))
//     };
// });

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();


//on doit enregistrer le service CarSeeder dans le conteneur dinjection de
//dependance pour quil soit disponible dans les controleurs
//on a pas eu a faire cet enregistrements pr model et controller pck 
//ASP.NET Core le prend en charge automatiquement grace a la convention MVC 
builder.Services.AddScoped<CarSeeder>();
builder.Services.AddScoped<RoleSeeder>();
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var roleSeeder = services.GetRequiredService<RoleSeeder>();
        await roleSeeder.SeedRolesAsync();
        Console.WriteLine("Roles and users seeded successfully.");
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the roles and users.");
    }
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//MiddleWare pour gerer lauthentification et lautorisation

app.UseAuthentication();
app.UseAuthorization();


app.Use(async (context, next) =>
{
    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
    logger.LogInformation("Request Path: {Path}", context.Request.Path);
    logger.LogInformation("Request Method: {Method}", context.Request.Method);
    await next.Invoke();
});

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
