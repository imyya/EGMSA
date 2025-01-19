using Microsoft.AspNetCore.Identity;

//namespace Models;
namespace EMGMSA.Models;


public class User : IdentityUser
{
// proprietes supplementaires
    public string FirstName { get; set; } 
    public string LastName { get; set; }  
   // public string Role { get; set; }
}