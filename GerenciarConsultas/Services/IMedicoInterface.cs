using GerenciarConsultas.DTO;
using GerenciarConsultas.Model;

public interface IMedicoInterface
{
    Task<ResponseModel<List<MedicoListarDTO>>> BuscarMedicos();
    Task<ResponseModel<MedicoListarDTO>> BuscarMedicoPorId(int medicoId);
    Task<ResponseModel<MedicoListarDTO>> CriarMedico(MedicoListarDTO medicoCriarDto); // Atualizado
    Task<ResponseModel<List<MedicoListarDTO>>> EditarMedico(MedicoEditarDto medicoEditarDto);
    Task<ResponseModel<List<MedicoListarDTO>>> RemoverMedico(int medicoId);
    Task<ResponseModel<MedicoListarDTO>> BuscarMedicoPorEmail(string email);
    Task<ResponseModel<List<PacienteDTO>>> BuscarPacientesPorMedico(int medicoId); 
}
