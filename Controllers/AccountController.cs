using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

using EMGMSA.Models;

public class AccountController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;

    private const string defaultRow = "user";

    public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _configuration = configuration;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }


    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]

    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var checkUser = await _userManager.FindByEmailAsync(model.Email);
            Console.WriteLine("DEBUG MODEL:");
            Console.WriteLine($"FirstName: '{model.FirstName}'");
            Console.WriteLine($"LastName: '{model.LastName}'");
            Console.WriteLine($"Email: '{model.Email}'");

            Console.WriteLine($"Form data received:");
            foreach (var key in Request.Form.Keys)
            {
                Console.WriteLine($"{key}: {Request.Form[key]}");
            }

            if (checkUser != null)
            {
                ModelState.AddModelError(string.Empty, "Email already exists");
                return View(model);
            }

            // var user = new User
            // {
            //     UserName = model.Email,
            //     Email = model.Email,
            //     FirstName = model.FirstName,
            //     LastName = model.LastName,
            //     //Role = "user"
            // };


            var user = new IdentityUser { UserName = model.Email, Email = model.Email };

            Console.WriteLine("\nDEBUG USER OBJECT:");
            //Console.WriteLine($"FirstName : '{user.FirstName}'");
            //Console.WriteLine($"LastName: '{user.LastName}'");
            Console.WriteLine($"Email: '{user.Email}'");

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, defaultRow);
                await _signInManager.SignInAsync(user, isPersistent: false);
                //var token = GenerateJwtToken(user);
                //return Ok(new {token});
                return RedirectToAction("Index", "Cars");

            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        return View(model);
    }


    [HttpPost]

    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            //var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
            var result = await _signInManager.PasswordSignInAsync(
            model.Email,
            model.Password,
            isPersistent: false,
            lockoutOnFailure: false);

            if (result.Succeeded)
            {
                Console.WriteLine("Login Success");
                Console.WriteLine($"Login successful. User authenticated: {User.Identity?.IsAuthenticated}");


                //var user = await _userManager.FindByEmailAsync(model.Email);
                //  await _signInManager.SignInAsync(user, isPersistent: false); // Enregistre l'utilisateur
                //var token = GenerateJwtToken(user);
                // return Ok(new { token });
                return RedirectToAction("Index", "Cars");


            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt");
        }

        return View(model);
    }

    // private string GenerateJwtToken(IdentityUser user)
    // {
    //     var claims = new[]
    //     {
    //         new Claim(JwtRegisteredClaimNames.Sub, user.Email),
    //         new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    //     };

    //     var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));
    //     var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    //     var token = new JwtSecurityToken(
    //         issuer: _configuration["JwtSettings:Issuer"],
    //         audience: _configuration["JwtSettings:Audience"],
    //         claims: claims,
    //         expires: DateTime.Now.AddMinutes(30),
    //         signingCredentials: creds
    //     );

    //     return new JwtSecurityTokenHandler().WriteToken(token);
    // }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login", "Account");
    }

}