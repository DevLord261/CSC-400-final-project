
using System.Collections.ObjectModel;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

[ApiController]
[Route("/api")]

public class Authentication : Controller
{


    private readonly UserManager<Users> _userManager;
    private readonly SignInManager<Users> _sign;
    private readonly IHttpContextAccessor _contextaccessor;
    private readonly RoleManager<IdentityRole> _rolemanager;
    private readonly TokenService _tokenService;
    public Authentication(UserManager<Users> userManager, SignInManager<Users> signInManager, IHttpContextAccessor context, RoleManager<IdentityRole> rolemanager, TokenService tokens)
    {
        _rolemanager = rolemanager;
        _contextaccessor = context;
        _userManager = userManager;
        _sign = signInManager;
        _tokenService = tokens;
    }


    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Registratoion([FromBody] Users _user)
    {
        var result = await _userManager.CreateAsync(_user, _user.password);
        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(_user, "User");
            await _sign.SignInAsync(_user, false);

            var nyuser = await _userManager.FindByNameAsync(_user.UserName);

            var token = _tokenService.GenerateToken(nyuser);

            return Ok(new { token = token });
        }

        return BadRequest(new { failed = result.Errors });
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] Users _user)
    {

        var result = await _sign.PasswordSignInAsync(_user.UserName, _user.password, false, false);
        if (result.Succeeded)
        {
            var nyuser = await _userManager.FindByNameAsync(_user.UserName);

            var token = _tokenService.GenerateToken(nyuser);
            return Ok(token);
        }
        return BadRequest(new { error = "login failed" });
    }

    [HttpPost("Signout")]

    public async Task<IActionResult> Signoutuser()
    {
        await _sign.SignOutAsync();
        return Ok();
    }

    [HttpPost("check")]
    public async Task<IActionResult> checkform([FromBody] Users _user)
    {
        var foundemail = await _userManager.FindByEmailAsync(_user.Email);
        var usernamexists = await _userManager.FindByNameAsync(_user.UserName);
        if (foundemail != null)
            return BadRequest(new { email = "email already exist" });
        if (usernamexists != null)
            return BadRequest(new { username = "usernmae already exist" });

        return Ok();
    }

    [HttpGet("islogin")]
    [Authorize]
    public async Task<IActionResult> isAuthenticate()
    {
        var principal = _contextaccessor.HttpContext.User;
        if (principal.Identity.IsAuthenticated)
        {

            var user = await _userManager.FindByNameAsync(principal.FindFirstValue(ClaimTypes.NameIdentifier));
            Users myuser = new Users
            {
                UserName = user.UserName,
                fname = user.fname,
                lname = user.lname,
                Email = user.Email
            };
            return Ok(new
            {
                User = myuser,
                authenticate = true
            });
        }
        return BadRequest("need to login");
    }


    [HttpPost("createrole")]
    [Authorize(Roles = "Admin")]
    public async Task<IdentityResult> CreateRole(string roleName)
    {
        if (String.IsNullOrEmpty(roleName))
        {

            return IdentityResult.Failed();
        }

        var roleexists = await _rolemanager.RoleExistsAsync(roleName);
        if (roleexists)
        {
            ModelState.AddModelError("", "Role already exists");
            return IdentityResult.Success;
        }
        var result = await _rolemanager.CreateAsync(new IdentityRole(roleName));
        if (result.Succeeded)
        {
            return IdentityResult.Success;
        }


        foreach (var error in result.Errors)
        {
            ModelState.AddModelError("", error.Description);
        }
        return IdentityResult.Failed();
    }

}
