using System.Net;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


[ApiController]
[Route("/api")]
[Authorize]
public class Campaigns : Controller
{
    private readonly AppDbContext _db;
    private readonly UserManager<Users> _userManager;
    private readonly IHttpContextAccessor _contextaccessor;

    public Campaigns(AppDbContext db, UserManager<Users> userManager, IHttpContextAccessor context)
    {
        _db = db;
        _userManager = userManager;
        _contextaccessor = context;

    }

    [HttpPost("CreateCampaign")]
    public async Task<IActionResult> createcampaign([FromBody] Campaign campaign)
    {
        try
        {
            var principal = _contextaccessor.HttpContext.User;
            var user = await _userManager.FindByNameAsync(principal.FindFirstValue(ClaimTypes.NameIdentifier));

            if (user == null)
            {
                return Unauthorized("User not found");
            }

            campaign.owner = user;
            await _db.AddAsync(campaign);
            var result = await _db.SaveChangesAsync();

            if (result > 0)
            {
                return Ok("successful");
            }

            return BadRequest("failed to save campaign");
        }
        catch (Exception ex)
        {
            // Log the exception (you could use a logging framework here)
            Console.WriteLine("Exception occurred while creating campaign: " + ex.Message);
            return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while creating the campaign.");
        }
    }

    [HttpGet("ownedcampaigns")]
    public async Task<List<Campaign>> getownedcampaigns()
    {
        if (!_contextaccessor.HttpContext.User.Identity.IsAuthenticated)
            return new List<Campaign>();
        var user = await _userManager.FindByNameAsync(_contextaccessor.HttpContext.User.Identity.Name);
        var result = await _db.campaigns.Where(u => u.owner == user).ToListAsync();

        return result;

    }

    [HttpGet("listcampaigns")]
    [AllowAnonymous]
    public async Task<List<Campaign>> getlistcampaigns()
    {

        var result = await _db.campaigns.ToListAsync();

        foreach (var campaign in result)
        {
            var user = await _userManager.FindByIdAsync(campaign.ownerId);
            campaign.owner = user;
        }
        return result;
    }


}