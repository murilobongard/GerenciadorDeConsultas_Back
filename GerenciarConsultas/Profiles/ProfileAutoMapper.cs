using AutoMapper;
using GerenciarConsultas.DTO;
using GerenciarConsultas.Model;

namespace GerenciarConsultas.Profiles
{
    public class ProfileAutoMapper : Profile
    {
        public ProfileAutoMapper() {
          
            CreateMap<Pacientes, PacienteDTO>(); 
            CreateMap<EditarPacienteDto, Pacientes>();

            CreateMap<Medicos, MedicoListarDTO>();
            CreateMap<MedicoEditarDto, Medicos>();

        }

    }
}
