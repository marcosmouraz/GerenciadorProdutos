using GerenciadorProdutos.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GerenciadorProdutos.Controllers;

[Route("[controller]")]
[ApiController]
public class UsuarioController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;

    public UsuarioController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
    }

    [HttpPost("registro")]
    public async Task<IActionResult> Registro([FromBody] Registro registro)
    {
        var user = new IdentityUser { UserName = registro.Username, Email = registro.Email };
        var result = await _userManager.CreateAsync(user, registro.Password);

        if (result.Succeeded)
        {
            return Ok(new { message = "Usuário registrado com sucesso" });
        }

        return BadRequest(result.Errors);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] Usuario model)
    {
        var user = await _userManager.FindByNameAsync(model.Username);
        if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName!),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

            authClaims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                expires: DateTime.Now.AddMinutes(double.Parse(_configuration["Jwt:ExpiryMinutes"]!)),
                claims: authClaims,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)),
                SecurityAlgorithms.HmacSha256));

            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        }

        return Unauthorized();
    }

    [Authorize(Roles = "Gerente,Admin")]
    [HttpPost("adicionar-role")]
    public async Task<IActionResult> AddRole([FromBody] string role)
    {
        if (!await _roleManager.RoleExistsAsync(role))
        {
            var result = await _roleManager.CreateAsync(new IdentityRole(role));
            if (result.Succeeded)
            {
                return Ok(new { message = "Role adicionada com sucesso" });
            }

            return BadRequest(result.Errors);
        }

        return BadRequest("Role já existe");
    }

    [Authorize(Roles = "Gerente")]
    [HttpPost("adicionar-role-ao-usuario")]
    public async Task<IActionResult> AssignRole([FromBody] UsuarioRole model)
    {
        var user = await _userManager.FindByNameAsync(model.Username);
        if (user == null)
        {
            return BadRequest("Usuário não existe");
        }

        var result = await _userManager.AddToRoleAsync(user, model.Role);
        if (result.Succeeded)
        {
            return Ok(new { message = $"Role {model.Role} adicionada com sucesso ao usuário ${model.Username}" });
        }

        return BadRequest(result.Errors);
    }

    [Authorize(Roles = "Gerente,Admin")]
    [HttpGet("listar-roles")]
    public IActionResult ListarRoles()
    {
        var roles = _roleManager.Roles.Select(r => new { r.Id, r.Name }).ToList();
        return Ok(roles);
    }

    [Authorize(Roles = "Gerente")]
    [HttpPost("remover-role-do-usuario")]
    public async Task<IActionResult> RemoverRoleDoUsuario([FromBody] UsuarioRole model)
    {
        var user = await _userManager.FindByNameAsync(model.Username);
        if (user == null)
        {
            return BadRequest("Usuário não existe.");
        }

        if (!await _userManager.IsInRoleAsync(user, model.Role))
        {
            return BadRequest("Usuário não possui essa role.");
        }

        var result = await _userManager.RemoveFromRoleAsync(user, model.Role);
        if (result.Succeeded)
        {
            return Ok(new { message = "Role removida com sucesso." });
        }

        return BadRequest(result.Errors);
    }
}
