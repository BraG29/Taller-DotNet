using Authentication.DTOS;
using Authentication.Models;
using Authentication.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly JwtTokenService _jwtTokenService;
    private readonly IUserRepository _userRepository;

    public AuthController(JwtTokenService jwtTokenService, IUserRepository userRepository)
    {
        _jwtTokenService = jwtTokenService;
        _userRepository = userRepository;
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> SignUp([FromBody] RegisterRequest request)
    {
        User user = new User
        {
            Id = 0,
            Username = request.Username,
            Password = request.Password,
            Name = request.Name,
            LastName = request.LastName,
            Role = request.Role
        };

        try
        {
            user = await _userRepository.Save(user);
            return Ok(new
            {
                message = "Usuario creado correctamente",
                id = user.Id
            });
        }
        catch (DbUpdateException e)
        {
            return BadRequest(new
            {
                error = $"El usuario {request.Username} ya existe"
            });
        }
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        User? user = await _userRepository.GetByUsernameAndPassword(request.Username, request.Password);

        if (user == null) return BadRequest("Username o Password incorrecto");
  
        var token = _jwtTokenService.GenerateToken(user.Id, user.Role.ToString().ToUpper());
        
        return Ok(new { Token = token });
    }
}