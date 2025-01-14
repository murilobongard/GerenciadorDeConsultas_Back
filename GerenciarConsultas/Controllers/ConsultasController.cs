using AutoMapper;
using GerenciarConsultas.DTO;
using GerenciarConsultas.Repository;
using Microsoft.AspNetCore.Mvc;

namespace GerenciarConsultas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConsultaController : ControllerBase
    {
        private readonly ConsultaService _consultaService;
        private readonly IMapper _mapper;

        public ConsultaController(ConsultaService consultaService, IMapper mapper)
        {
            _consultaService = consultaService;
            _mapper = mapper;
        }

        // Buscar todas as consultas
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var consultas = await _consultaService.BuscarConsultas();
            if (!consultas.Status)
            {
                return NotFound(consultas.Mensagem);
            }

            var consultaDTOs = _mapper.Map<IEnumerable<ConsultaDTO>>(consultas.Dados);
            return Ok(consultaDTOs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var consultaResponse = await _consultaService.BuscarConsultaPorId(id);

            if (!consultaResponse.Status)
            {
                return NotFound(consultaResponse.Mensagem);
            }

            var consultaDTO = _mapper.Map<ConsultaDTO>(consultaResponse.Dados);

            return Ok(consultaDTO);
        }

        // Criar uma nova consulta
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ConsultaCriarDtos consultaCriarDto)
        {
            try
            {
                var consultaResponse = await _consultaService.CriarConsulta(consultaCriarDto);

                if (!consultaResponse.Status)
                {
                    return BadRequest(consultaResponse.Mensagem);
                }

                // Garantir que a consulta criada não seja null
                var consultaCriada = consultaResponse.Dados?.FirstOrDefault();

                if (consultaCriada == null)
                {
                    return BadRequest("Erro ao criar consulta.");
                }

                // Mapeia corretamente para o DTO
                var consultaDTO = _mapper.Map<ConsultaDTO>(consultaCriada);
                return CreatedAtAction(nameof(GetById), new { id = consultaCriada.Id }, consultaDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Editar consulta existente
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ConsultaEditarDto consultaEditarDto)
        {
            try
            {
                // Verifica se o ID da URL corresponde ao ID da consulta no banco de dados
                var sucesso = await _consultaService.EditarConsulta(id, consultaEditarDto);
                if (!sucesso.Status)
                {
                    return NotFound(sucesso.Mensagem);
                }

                var consultaDTO = _mapper.Map<ConsultaDTO>(sucesso.Dados);
                return Ok(consultaDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("medico/{medicoId}/pacientes")]
        public async Task<IActionResult> GetPacientesPorMedico(int medicoId)
        {
            try
            {
                var pacientesResponse = await _consultaService.BuscarPacientesPorMedico(medicoId);

                if (!pacientesResponse.Status)
                {
                    return NotFound(pacientesResponse.Mensagem);
                }

                var pacientesDTOs = _mapper.Map<IEnumerable<PacienteDTO>>(pacientesResponse.Dados);
                return Ok(pacientesDTOs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        

        // Remover consulta
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var sucesso = await _consultaService.RemoverConsulta(id);
                if (!sucesso.Status) return NotFound(sucesso.Mensagem);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}