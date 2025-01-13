using AutoMapper;
using Dapper;
using GerenciarConsultas.DTO;
using GerenciarConsultas.Model;
using Microsoft.Data.SqlClient;


namespace GerenciarConsultas.Services
{
    public class PacienteService : IPacienteInterface
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public PacienteService(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<ResponseModel<List<PacienteDTO>>> BuscarPacientes()
        {
            ResponseModel<List<PacienteDTO>> response = new ResponseModel<List<PacienteDTO>>();

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var pacientesBanco = await connection.QueryAsync<Pacientes>("select * from Pacientes");

                if (pacientesBanco.Count() == 0)
                {
                    response.Mensagem = "Nenhum paciente localizado!";
                    response.Status = false;
                    return response;
                }

                var pacienteMapeado = _mapper.Map<List<PacienteDTO>>(pacientesBanco);
                response.Dados = pacienteMapeado;
                response.Mensagem = "Paciente localizado!";
            }

            return response;
        }

        public async Task<ResponseModel<PacienteDTO>> BuscarPacientesId(int pacienteId)
        {
            var response = new ResponseModel<PacienteDTO>();

            try
            {
                // Estabelecendo conexão com o banco de dados
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    // Buscando o paciente pelo ID
                    var pacienteBanco = await connection.QueryFirstOrDefaultAsync<Pacientes>(
                        "SELECT * FROM Pacientes WHERE Id = @Id", new { Id = pacienteId });

                    // Se o paciente não for encontrado
                    if (pacienteBanco == null)
                    {
                        response.Status = false;
                        response.Mensagem = "Paciente não encontrado!";
                        return response;
                    }

                    // Mapeando para o DTO
                    var pacienteDTO = _mapper.Map<PacienteDTO>(pacienteBanco);

                    // Configurando o retorno
                    response.Status = true;
                    response.Mensagem = "Paciente encontrado!";
                    response.Dados = pacienteDTO;
                }
            }
            catch (Exception ex)
            {
                // Tratamento de erros
                response.Status = false;
                response.Mensagem = $"Erro ao buscar paciente: {ex.Message}";
            }

            return response;
        }



        public async Task<ResponseModel<PacienteDTO>> CriarPaciente(PacienteCriarDto pacienteCriarDto)
        {
            ResponseModel<PacienteDTO> response = new ResponseModel<PacienteDTO>();

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var pacienteBanco = await connection.ExecuteAsync(
                   
                   "INSERT INTO Pacientes(Nome, DataNascimento, Telefone, CPF, Email, Senha)"+
                    "VALUES(@Nome, @DataNascimento, @Telefone, @CPF, @Email, @Senha)", pacienteCriarDto);

                if (pacienteBanco == 0)
                {
                    response.Mensagem = "Ocorreu um erro ao cadastrar um paciente!";
                    response.Status = false;
                    return response;
                }

                var pacientes = await ListarPacientes(connection);

                var pacientesMapeados = _mapper.Map<List<PacienteDTO>>(pacientes);
                response.Dados = pacientesMapeados.FirstOrDefault();  // Pega o primeiro paciente da lista
                response.Mensagem = "Paciente criado com sucesso!";
            }

            return response;
        }

        private static async Task<IEnumerable<Pacientes>> ListarPacientes(SqlConnection connection)
        {
            var query = "SELECT * FROM Pacientes"; 
            var pacientes = await connection.QueryAsync<Pacientes>(query); 
            return pacientes.ToList();
        }


        public async Task<ResponseModel<List<PacienteCriarDto>>> EditarPaciente(EditarPacienteDto editarPacienteDto)
        {
            ResponseModel<List<PacienteCriarDto>> response = new ResponseModel<List<PacienteCriarDto>>();

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var pacienteBanco = await connection.ExecuteAsync(
                    "UPDATE Pacientes SET Nome = @Nome, Email = @Email, Telefone = @Telefone, CPF = @CPF, DataNascimento = @DataNascimento WHERE Id = @Id",
                    editarPacienteDto
                );

                if (pacienteBanco == 0)
                {
                    response.Mensagem = "Ocorreu um erro ao atualizar o paciente.";
                    response.Status = false;
                    return response;
                }

                
                var pacientes = await ListarPacientes(connection);

                
                var pacientesMapeados = _mapper.Map<List<PacienteCriarDto>>(pacientes);

                response.Dados = pacientesMapeados;
                response.Mensagem = "Pacientes listados com sucesso.";
            }

            return response;
        }

        public async Task<ResponseModel<List<PacienteCriarDto>>> RemoverPaciente(int pacienteId)
        {
            ResponseModel<List<PacienteCriarDto>> response = new ResponseModel<List<PacienteCriarDto>>();

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                    var pacienteBanco = await connection.ExecuteAsync("delete from Pacientes where Id = @Id", new { Id = pacienteId });

                if (pacienteBanco == 0)
                {
                    response.Mensagem = "Ocorreu um erro ao deletar o paciente.";
                    response.Status = false;
                    return response;
                }
                var pacientes = await ListarPacientes(connection);

                var pacientesMapeados = _mapper.Map<List<PacienteCriarDto>>(pacientes);
                response.Dados = pacientesMapeados;
                response.Mensagem = "Paciente deletado com sucesso";

            }
            return response;
        }
    }
}