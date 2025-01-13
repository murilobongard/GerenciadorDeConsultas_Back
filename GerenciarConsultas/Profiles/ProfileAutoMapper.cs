using AutoMapper;
using GerenciarConsultas.DTO;
using GerenciarConsultas.Model;

namespace GerenciarConsultas.Profiles
{
    public class ProfileAutoMapper : Profile
    {
        public ProfileAutoMapper() {

            CreateMap<ConsultaListaDT0, ConsultaCriarDtos>()
               .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Data))
               .ForMember(dest => dest.ValorConsulta, opt => opt.MapFrom(src => src.ValorConsulta))
               .ForMember(dest => dest.MedicoId, opt => opt.MapFrom(src => src.MedicoId))
               .ForMember(dest => dest.PacienteId, opt => opt.MapFrom(src => src.PacienteId))
               .ForMember(dest => dest.Observacoes, opt => opt.MapFrom(src => src.Observacoes))
               .ForMember(dest => dest.TipoConsulta, opt => opt.MapFrom(src => src.TipoConsulta));
            CreateMap<ConsultaListaDT0, ConsultaDTO>();
            CreateMap<Consulta, ConsultaListaDT0>();
            CreateMap<Consulta, ConsultaDTO>();
            CreateMap<ConsultaDTO, Consulta>();
            CreateMap<Pacientes, PacienteDTO>();
           
            CreateMap<Medicos, MedicoListarDTO>();
            CreateMap<MedicoEditarDto, Medicos>();

        }

    }
}
