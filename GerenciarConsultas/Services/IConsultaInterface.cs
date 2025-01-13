using GerenciarConsultas.DTO;
using GerenciarConsultas.Model;


namespace GerenciarConsultas.Services
{
    public interface IConsultaInterface
    {
        Task<ResponseModel<List<ConsultaListaDT0>>> BuscarConsultas();
        Task<ResponseModel<ConsultaListaDT0>> BuscarConsultaPorId(int consultaId);
        Task<ResponseModel<List<ConsultaListaDT0>>> CriarConsulta(ConsultaCriarDtos consultaCriarDto);
        Task<ResponseModel<Consulta>> EditarConsulta(int id, ConsultaEditarDto consultaEditarDto);
        Task<ResponseModel<List<ConsultaListaDT0>>> RemoverConsulta(int consultaId);
    }
}
