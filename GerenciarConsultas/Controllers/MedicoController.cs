using GerenciarConsultas.DTO;
using GerenciarConsultas.Services;
using Microsoft.AspNetCore.Mvc;

namespace GerenciarConsultas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicoController : ControllerBase
    {
        private readonly IMedicoInterface _mediicoInterface;
        public  MedicoController(IMedicoInterface mediicoInterface)
        {
            _mediicoInterface = mediicoInterface;
        }

        [HttpGet]
        public async Task<IActionResult> BuscarUsuarios()
        {
            var medicos = await _mediicoInterface.BuscarMedicos();  

            if(medicos.Status == false)
            {
                return NotFound(medicos);
            }
            return Ok(medicos);
        }


        [HttpGet("{medicoId}")]
        public async Task<IActionResult> BuscarMedicoPorId(int medicoId)
        {
            var medicos = await _mediicoInterface.buscarMediscoPorId(medicoId);

            if (medicos.Status == false)
            {
                return NotFound(medicos);
            }
            return Ok(medicos);
        }

        [HttpPost]
        public async Task<IActionResult> CriarMedico(MedicoCriarDto medicoCriarDto)
        {
            var medicos = await _mediicoInterface.CriarMedico(medicoCriarDto);

            if(medicos.Status == false)
            {
                return BadRequest(medicos);   
            }
            return Ok(medicos);
        }

        [HttpPut]
        public async Task<IActionResult> EditarUsuario(MedicoEditarDto medicoEditarDto)
        {
            var medicos = await _mediicoInterface.EditarMedico(medicoEditarDto);

            if (medicos.Status == false)
            {
                return NotFound(medicos);
            }
            return Ok(medicos);
        }

        [HttpDelete]
        public async Task<IActionResult> RemoverMedicos(int medicoId)
        {
            var medicos = await _mediicoInterface.RemoverMedico(medicoId);

            if (medicos.Status == false)
            {
                return NotFound(medicoId);
            }
            return Ok(medicos);
        }

    }
}
