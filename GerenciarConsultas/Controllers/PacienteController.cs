using GerenciarConsultas.DTO;
using GerenciarConsultas.Model;
using GerenciarConsultas.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GerenciarConsultas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PacienteController : ControllerBase
    {
        private readonly IPacienteInterface _pacienteInterface;

        public PacienteController(IPacienteInterface pacienteInterface)
        {
            _pacienteInterface = pacienteInterface;
        }

        [HttpGet]
        public async Task<IActionResult> buscarPaciente()
        {
            var pacientes = await _pacienteInterface.BuscarPacientes();

            if (pacientes.Status == false)
            {
                return NotFound(pacientes);
            }

            return Ok(pacientes);
        }

        [HttpGet("{pacienteId}")]
        public async Task<IActionResult> BuscarPacientePorId(int pacienteId)
        {
            var paciente = await _pacienteInterface.BuscarPacientesId(pacienteId);

            if (paciente.Status == false)
            {
                return NotFound(paciente);
            }
            return Ok(paciente);
        }
        [HttpPost]
        public async Task<IActionResult> CriarPaciente(PacienteCriarDto pacienteCriarDto)
        {
            var pacientes = await _pacienteInterface.CriarPaciente(pacienteCriarDto);
            if (pacientes.Status == false)
            {
                return BadRequest(pacientes);
            }
            return Ok(pacientes);
        }
        [HttpPut]
        public async Task<IActionResult> EditarPaciente(EditarPacienteDto editarPacienteDto)
        {
            var pacientes = await _pacienteInterface.EditarPaciente(editarPacienteDto);

            if (pacientes.Status == false)
            {
                return BadRequest(pacientes);
            }
            return Ok(pacientes);
        }


        [HttpDelete]
        public async Task<IActionResult> deletarPaciente(int ususarioId)
        {
            var pacientes = await _pacienteInterface.RemoverPaciente(ususarioId);

            if (pacientes.Status == false) { return BadRequest(pacientes); }
            return Ok(pacientes);
        }

    }
    
}
