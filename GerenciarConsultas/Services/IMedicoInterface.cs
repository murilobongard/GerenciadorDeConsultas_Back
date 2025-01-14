using GerenciarConsultas.DTO;
using GerenciarConsultas.Model;

public interface IMedicoInterface
{
    Task<ResponseModel<List<MedicoListarDTO>>> BuscarMedicos();
    Task<ResponseModel<MedicoListarDTO>> BuscarMedicoPorId(int medicoId);
    Task<ResponseModel<List<MedicoListarDTO>>> CriarMedico(MedicoCriarDto criarMedicoDto);
    Task<ResponseModel<List<MedicoListarDTO>>> EditarMedico(MedicoEditarDto medicoEditarDto);
    Task<ResponseModel<List<MedicoListarDTO>>> RemoverMedico(int medicoId);
    Task<ResponseModel<MedicoListarDTO>> BuscarMedicoPorEmail(string email);
    Task<ResponseModel<List<PacienteDTO>>> BuscarPacientesPorMedico(int medicoId); // Add this method
}
