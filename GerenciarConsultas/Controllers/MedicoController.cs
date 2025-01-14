using GerenciarConsultas.DTO;
using GerenciarConsultas.Services;
using Microsoft.AspNetCore.Mvc;

namespace GerenciarConsultas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicoController : ControllerBase
    {
        private readonly IMedicoInterface _medicoService;

        public MedicoController(IMedicoInterface medicoService)
        {
            _medicoService = medicoService;
        }

        [HttpGet]
        public async Task<IActionResult> BuscarMedicos()
        {
            var response = await _medicoService.BuscarMedicos();
            if (!response.Status)
            {
                return NotFound(response.Mensagem);
            }
            return Ok(response.Dados);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarMedicoPorId(int id)
        {
            var response = await _medicoService.BuscarMedicoPorId(id);
            if (!response.Status)
            {
                return NotFound(response.Mensagem);
            }
            return Ok(response.Dados);
        }

        [HttpPost]
        public async Task<IActionResult> CriarMedico([FromBody] MedicoCriarDto medicoCriarDto)
        {
            var response = await _medicoService.CriarMedico(medicoCriarDto);
            if (!response.Status)
            {
                return BadRequest(response.Mensagem);
            }
            return Ok(response.Dados);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditarMedico(int id, [FromBody] MedicoEditarDto medicoEditarDto)
        {
            medicoEditarDto.Id = id; // Certifique-se de que o ID está correto
            var response = await _medicoService.EditarMedico(medicoEditarDto);
            if (!response.Status)
            {
                return NotFound(response.Mensagem);
            }
            return Ok(response.Dados);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoverMedico(int id)
        {
            var response = await _medicoService.RemoverMedico(id);
            if (!response.Status)
            {
                return NotFound(response.Mensagem);
            }
            return NoContent();
        }

        [HttpGet("email/{email}")]
        public async Task<IActionResult> BuscarMedicoPorEmail(string email)
        {
            var response = await _medicoService.BuscarMedicoPorEmail(email);
            if (!response.Status)
            {
                return NotFound(response.Mensagem);
            }
            return Ok(response.Dados);
        }

        [HttpGet("pacientes/{medicoId}")]
        public async Task<IActionResult> BuscarPacientesPorMedico(int medicoId)
        {
            var response = await _medicoService.BuscarPacientesPorMedico(medicoId);
            if (!response.Status)
            {
                return NotFound(response.Mensagem);
            }
            return Ok(response.Dados);
        }
    }
}