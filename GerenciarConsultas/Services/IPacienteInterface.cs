using GerenciarConsultas.DTO;
using GerenciarConsultas.Model;

namespace GerenciarConsultas.Services
{
    public interface IPacienteInterface
    {
        Task<ResponseModel<List<PacienteDTO>>> BuscarPacientes();
        Task<ResponseModel<PacienteDTO>> BuscarPacientesId(int pacienteId);
        Task<ResponseModel<PacienteDTO>> CriarPaciente(PacienteCriarDto pacienteCriarDto);
        Task<ResponseModel<PacienteDTO>> EditarPaciente(int id, EditarPacienteDto editarPacienteDto); // Alterado para aceitar 2 argumentos
        Task<ResponseModel<List<PacienteCriarDto>>> RemoverPaciente(int pacienteId);
        Task<ResponseModel<PacienteDTO>> BuscarPacientePorEmail(string email);
        Task<ResponseModel<List<PacienteDTO>>> BuscarPacientesPorMedico(int medicoId);
    }

}
