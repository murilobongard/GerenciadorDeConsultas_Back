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

            CreateMap<Medicos, MedicoListarDTO>()
           .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
           .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Nome))
           .ForMember(dest => dest.Especialidade, opt => opt.MapFrom(src => src.Especialidade))
           .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));
            CreateMap<MedicoEditarDto, Medicos>();

            CreateMap<PacienteDTO, Pacientes>();
            CreateMap<MedicoCriarDto, Medicos>();

        }

    }
}
