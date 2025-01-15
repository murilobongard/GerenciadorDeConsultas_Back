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

        [HttpGet("medico/{medicoId}")]
        public async Task<IActionResult> BuscarPacientesPorMedico(int medicoId)
        {
            var pacientes = await _pacienteInterface.BuscarPacientesPorMedico(medicoId);

            if (!pacientes.Status)
            {
                return NotFound(pacientes);
            }

            return Ok(pacientes);
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
        [HttpPut("{id}")]
        public async Task<IActionResult> EditarPaciente(int id, [FromBody] EditarPacienteDto editarPacienteDto)
        {
            var pacienteAtualizado = await _pacienteInterface.EditarPaciente(id, editarPacienteDto);

            if (pacienteAtualizado.Status == false)
            {
                return BadRequest(pacienteAtualizado);
            }
            return Ok(pacienteAtualizado);
        }


        [HttpDelete]
        public async Task<IActionResult> deletarPaciente(int ususarioId)
        {
            var pacientes = await _pacienteInterface.RemoverPaciente(ususarioId);

            if (pacientes.Status == false) { return BadRequest(pacientes); }
            return Ok(pacientes);
        }

        [HttpGet("email/{email}")]
        public async Task<IActionResult> BuscarPacientePorEmail(string email)
        {
            var paciente = await _pacienteInterface.BuscarPacientePorEmail(email);

            if (paciente.Status == false)
            {
                return NotFound(paciente);
            }
            return Ok(paciente);
        }
    }

}
