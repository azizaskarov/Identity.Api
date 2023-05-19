using System.Security.Claims;
using Identity.Api.Context;
using Identity.Api.Entities;
using Identity.Api.Models;
using Identity.Api.Services;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Identity.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<AccountsController> _logger;
    private readonly TokenService _tokenService;

    public AccountsController(AppDbContext context, ILogger<AccountsController> logger, TokenService tokenService)
    {
        _context = context;
        _logger = logger;
        _tokenService = tokenService;
    }

    [HttpPost]
    public async Task<IActionResult> SignUp([FromBody] CreateUserModel userModel)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if(await _context.Users.AnyAsync(u => u.Username == userModel.Username))
            return BadRequest();

        var user = userModel.Adapt<User>();

         _context.Users.Add(user);
         await _context.SaveChangesAsync();

         return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserModel loginUserModel)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == loginUserModel.UserName);

        if (user == null && user.Password != loginUserModel.UserName)
        {
            return NotFound();
        }

        var token = _tokenService.GenerateToken(user);


        return Ok(token);
    }

    [HttpGet("profile")]
    [Authorize]
    public async Task<IActionResult> Profile()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id  == userId);

        return Ok(user);
    }
}