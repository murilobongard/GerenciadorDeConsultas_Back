using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using GerenciarConsultas.Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using BCrypt.Net;
using GerenciarConsultas.Model;

namespace GerenciarConsultas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IPacienteInterface _pacienteService;
        private readonly IMedicoInterface _medicoService;
        private readonly IConfiguration _configuration;

        public LoginController(IPacienteInterface pacienteService, IMedicoInterface medicoService, IConfiguration configuration)
        {
            _pacienteService = pacienteService;
            _medicoService = medicoService;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (request == null)
            {
                return BadRequest("O corpo da requisição está vazio.");
            }

            if (string.IsNullOrEmpty(request.Email))
            {
                return BadRequest("O campo email é obrigatório.");
            }

            if (string.IsNullOrEmpty(request.Role))
            {
                return BadRequest("O campo role é obrigatório.");
            }

            if (request.Role == "medico")
            {
                var medico = await _medicoService.BuscarMedicoPorEmail(request.Email);
                if (medico == null || medico.Dados == null || !BCrypt.Net.BCrypt.Verify(request.Password, medico.Dados.Senha))
                {
                    return Unauthorized("Credenciais inválidas.");
                }

                var token = GenerateJwtToken(medico.Dados.Email);
                Console.WriteLine($"Id do médico: {medico.Dados.Id}");
                return Ok(new { token, role = "medico", id = medico.Dados.Id });

            }

            if (request.Role == "paciente")
            {
                var pacienteResponse = await _pacienteService.BuscarPacientePorEmail(request.Email);

                if (pacienteResponse.Status == false || !BCrypt.Net.BCrypt.Verify(request.Password, pacienteResponse.Dados.Senha))
                {
                    return Unauthorized("Credenciais inválidas.");
                }

                var token = GenerateJwtToken(pacienteResponse.Dados.Email);

                // Retornar o ID do paciente também
                return Ok(new { token, role = "paciente", id = pacienteResponse.Dados.Id });
            }

            return BadRequest("Role inválida.");
        }


        private string GenerateJwtToken(string email)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: new[] { new System.Security.Claims.Claim("email", email) },
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class LoginRequest
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
    }
}