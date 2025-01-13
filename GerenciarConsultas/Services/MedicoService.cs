using Microsoft.Data.SqlClient;
using AutoMapper;
using Dapper;
using GerenciarConsultas.DTO;
using GerenciarConsultas.Model;

namespace GerenciarConsultas.Services
{
    public class MedicoService : IMedicoInterface
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public MedicoService(IConfiguration configuration, IMapper mapper) { 
             
            _configuration = configuration;
            _mapper = mapper;
        }

     
        public async Task<ResponseModel<List<MedicoListarDTO>>> BuscarMedicos()
        {

            ResponseModel<List<MedicoListarDTO>> response = new ResponseModel<List<MedicoListarDTO>>();

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var medicosBanco = await connection.QueryAsync<Medicos>("select * from Medicos");

                if (medicosBanco.Count() == 0)
                {
                    response.Mensagem = "Nenhum Medico localizado";
                    response.Status = false;
                    return response;
                }

                var medicosMapeados = _mapper.Map<List<MedicoListarDTO>>(medicosBanco);
                response.Dados = medicosMapeados;
                response.Mensagem = "Medicos Localizados!!";
            }
            return response;
        }

        public async Task<ResponseModel<MedicoListarDTO>> buscarMediscoPorId(int medicoId)
        {
            ResponseModel<MedicoListarDTO> response = new ResponseModel<MedicoListarDTO>();

            using(var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var medicoBanco = await connection.QueryFirstOrDefaultAsync<Medicos>("select * from Medicos where Id = @Id", new {Id = medicoId});

                if (medicoBanco == null)
                {
                    response.Mensagem = "Nenhum medico locvalizado";
                    response.Status = false;
                    return response;
                }

                var medicosMapeados = _mapper.Map<MedicoListarDTO>(medicoBanco);
                response.Dados = medicosMapeados;
                response.Mensagem = "Medico Localizado com sucesso!!";
            }

            return response;


        }

        public async Task<ResponseModel<List<MedicoListarDTO>>> CriarMedico(MedicoCriarDto criarMedicoDto)
        {
            ResponseModel<List<MedicoListarDTO>> response = new ResponseModel<List<MedicoListarDTO>>();

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var medicosBanco = await connection.ExecuteAsync(
                    "INSERT INTO Medicos (Nome, Email, CRM, Senha, Especialidade) VALUES (@Nome, @Email, @CRM, @Senha, @Especialidade)",
                    criarMedicoDto
                );

                if (medicosBanco == 0)
                {
                    response.Mensagem = "Erro ao criar médico.";
                    response.Status = false;
                    return response;
                }

                var medicos = await ListarMedicos(connection); // Método que retorna a lista de médicos

                if (medicos == null || !medicos.Any())
                {
                    response.Mensagem = "Nenhum médico encontrado após inserção.";
                    response.Status = false;
                    response.Dados = new List<MedicoListarDTO>();
                    return response;
                }

                var medicosMapeados = _mapper.Map<List<MedicoListarDTO>>(medicos);

                response.Dados = medicosMapeados;
                response.Mensagem = "Médico criado com sucesso.";
                response.Status = true;
            }

            return response;
        }


        private static async Task<IEnumerable<Medicos>> ListarMedicos(SqlConnection connection)
        {

            var query = "select * from Medicos";
            var medicos = await connection.QueryAsync<Medicos>(query);
            return medicos.ToList();
        }

        public async Task<ResponseModel<List<MedicoListarDTO>>> EditarMedico(MedicoEditarDto medicoEditarDto)
        {
            ResponseModel<List<MedicoListarDTO>> response = new ResponseModel<List<MedicoListarDTO>>();

             using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var medicoBanco = await connection.ExecuteAsync("update Medicos set Nome = @Nome, Email = @Email, CRM = @CRM, Especialidade = @Especialidade where Id = @Id", medicoEditarDto);
                
                if(medicoBanco == 0)
                {
                    response.Mensagem = "Ocorreu um erro ao editar";
                    response.Status = false;
                    return response;
                }

                var medicos = await ListarMedicos(connection);

                var usuarioMapeados = _mapper.Map<List<MedicoListarDTO>>(medicos);

                response.Dados = usuarioMapeados;
                response.Mensagem = "Medicos Listados com sucesso!";


            }
             return response;
        }

        public async Task<ResponseModel<List<MedicoListarDTO>>> RemoverMedico(int medicoId)
        {
            ResponseModel<List<MedicoListarDTO>> response = new ResponseModel<List<MedicoListarDTO>>();

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var madicoBanco = await connection.ExecuteAsync("delete from Medicos where Id = @Id", new {Id = medicoId});
                
                if (madicoBanco == 0)
                {
                    response.Mensagem = "Ocorreu um erro ao remover";
                    response.Status = false;
                    return response;
                }

                var medicos = await ListarMedicos(connection);

                var medicosMapeados = _mapper.Map<List<MedicoListarDTO>>(medicos);

                response.Dados = medicosMapeados;
                response.Mensagem = "Medicos deletado com sucesso";
            }
            return response;    
        }
    }
}
