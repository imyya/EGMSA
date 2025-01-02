using Microsoft.AspNetCore.Identity;

public class User : IdentityUser
{
// proprietes supplementaires
    public string FirstName { get; set; } 
    public string LastName { get; set; }  
    public string Role { get; set; }
}