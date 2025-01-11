using GerenciarConsultas.DTO;
using GerenciarConsultas.Model;

namespace GerenciarConsultas.Services
{
    public interface IMedicoInterface
    {
        Task<ResponseModel<List<MedicoListarDTO>>> BuscarMedicos();
        Task<ResponseModel<MedicoListarDTO>> buscarMediscoPorId(int medicoId);
        Task<ResponseModel<List<MedicoListarDTO>>> CriarMedico(MedicoCriarDto criarMedicoDto);
        Task<ResponseModel<List<MedicoListarDTO>>> EditarMedico(MedicoEditarDto medicoEditarDto);
        Task<ResponseModel<List<MedicoListarDTO>>> RemoverMedico(int medicoId);
    }

}
