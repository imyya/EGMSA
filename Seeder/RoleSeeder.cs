using Microsoft.AspNetCore.Identity;

public class RoleSeeder
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<IdentityUser> _userManager;

    public RoleSeeder(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public async Task SeedRolesAsync()
    {
        string[] roles = ["admin", "user"];
        foreach (var role in roles)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        var adminEmail = "admin@emg.com";
        var adminPassword = "Admin@123";
        
        var userEmail = "mya@emg.com";
        var userPassword = "User@123";

        var admin = await _userManager.FindByEmailAsync("admin@emg.com");
        var user = await _userManager.FindByEmailAsync("mya@emg.com");

        if(user == null)
        {
            user = new IdentityUser
            {
                UserName = userEmail,
                Email = userEmail
            };

            var userCheck = await _userManager.CreateAsync(user, userPassword);
            if (userCheck.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "user");
            }
        }
        else if (!await _userManager.IsInRoleAsync(user, "user"))
        {
            await _userManager.AddToRoleAsync(user, "user");
        }


        
        if (admin == null)
        {
            admin = new IdentityUser
            {
                UserName = adminEmail,
                Email = adminEmail
            };

            var adminCheck = await _userManager.CreateAsync(admin, adminPassword);

            if (adminCheck.Succeeded)
            {
                await _userManager.AddToRoleAsync(admin, "admin");
            }
        }
        else if (!await _userManager.IsInRoleAsync(admin, "user"))
        {
            await _userManager.AddToRoleAsync(admin, "admin");
        }

    }


}