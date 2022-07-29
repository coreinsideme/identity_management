using Microsoft.AspNetCore.Mvc;
using IdentityManagement.Services;
using IdentityManagement.Entities;

namespace IdentityManagement.Controllers;

[ApiController]
[Route("[controller]")]
public class IdentityController : ControllerBase
{
    private readonly IUserValidator _validator;
    private readonly ITokenService _tokenService;

    public IdentityController(IUserValidator validator, ITokenService tokenService)
    {
        _validator = validator;
        _tokenService = tokenService;
    }

    [HttpPost("authorize")]
    public IActionResult GetToken([FromBody] AuthorizationRequestModel model)
    {
        var user = _validator.ValidateUser(model);

        if (user == null)
        {
            return BadRequest(new ErrorResponse { Message = "Wrong name or password", Status = "Error" });
        }

        return Ok(_tokenService.GetToken(user));
    }

    [HttpPost("validate")]
    public IActionResult ValidateToken([FromBody] string token)
    {
        return _tokenService.ValidateToken(token) ? Ok() : NotFound();
    }
}
