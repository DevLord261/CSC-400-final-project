
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

public class Users : IdentityUser
{

    public string? fname { get; set; }

    public string? lname { get; set; }

    public string? password { get; set; }

    public string? Role { get; set; }


}