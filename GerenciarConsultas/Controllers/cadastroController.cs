using GerenciarConsultas.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiConsultas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CadastroController : ControllerBase
    {
        private static List<Pacientes> _pacientes = new List<Pacientes>();
        private static List<Medicos> _medicos = new List<Medicos>();

    
        [HttpPost("paciente")]
        public async Task<ActionResult> CadastrarPaciente([FromBody] Pacientes paciente)
        {
            if (paciente == null)
            {
                return BadRequest("Paciente não cadastrado.");
            }

           
            _pacientes.Add(paciente);

            return CreatedAtAction(nameof(CadastrarPaciente), new { id = paciente.Id }, paciente);
        }

      
        [HttpPost("medico")]
        public async Task<ActionResult> CadastrarMedico([FromBody] Medicos medico)
        {
            if (medico == null)
            {
                return BadRequest("Médico não cadastrado.");
            }

            _medicos.Add(medico);

            return CreatedAtAction(nameof(CadastrarMedico), new { id = medico.Id }, medico);
        }
    }
}
