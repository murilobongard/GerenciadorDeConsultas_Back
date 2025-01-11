using AutoMapper;
using GerenciarConsultas.DTO;
using GerenciarConsultas.Model;

namespace GerenciarConsultas.Profiles
{
    public class ProfileAutoMapper : Profile
    {
        public ProfileAutoMapper() {
          
            CreateMap<Pacientes, PacienteCriarDto>(); ;
            CreateMap<EditarPacienteDto, Pacientes>();

        }

    }
}
