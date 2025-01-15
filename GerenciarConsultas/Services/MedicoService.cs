using Microsoft.Data.SqlClient;
using AutoMapper;
using Dapper;
using GerenciarConsultas.DTO;
using GerenciarConsultas.Model;
using System.Data;

namespace GerenciarConsultas.Services
{
    public class MedicoService : IMedicoInterface
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IDbConnection _dbConnection;

        public MedicoService(IConfiguration configuration, IMapper mapper, IDbConnection dbConnection)
        {
            _configuration = configuration;
            _mapper = mapper;
            _dbConnection = dbConnection;
        }

        // Existing methods...

        public async Task<ResponseModel<List<MedicoListarDTO>>> BuscarMedicos()
        {
            var response = new ResponseModel<List<MedicoListarDTO>>();
            var medicos = await _dbConnection.QueryAsync<Medicos>("SELECT * FROM Medicos");
            response.Dados = _mapper.Map<List<MedicoListarDTO>>(medicos.ToList());
            response.Mensagem = "Médicos encontrados com sucesso!";
            return response;
        }

        public async Task<ResponseModel<MedicoListarDTO>> BuscarMedicoPorId(int medicoId)
        {
            var response = new ResponseModel<MedicoListarDTO>();
            var medico = await _dbConnection.QueryFirstOrDefaultAsync<Medicos>(
                "SELECT * FROM Medicos WHERE Id = @Id", new { Id = medicoId });
            if (medico == null)
            {
                response.Mensagem = "Médico não encontrado.";
                response.Status = false;
                return response;
            }
            response.Dados = _mapper.Map<MedicoListarDTO>(medico);
            response.Mensagem = "Médico encontrado com sucesso!";
            return response;
        }

        public async Task<ResponseModel<MedicoListarDTO>> CriarMedico(MedicoListarDTO medicoCriarDto) // Alterado para usar MedicoListarDTO
        {
            var response = new ResponseModel<MedicoListarDTO>();

            // Mapeando DTO para o modelo de dados Medico
            var medico = new Medicos
            {
                Nome = medicoCriarDto.Nome,
                CRM = medicoCriarDto.CRM,
                Email = medicoCriarDto.Email,
                Senha = medicoCriarDto.Senha,
                Especialidade = medicoCriarDto.Especialidade
            };

            // Verificando se o médico já existe pelo CRM ou email
            var medicoExistente = await _dbConnection.QueryFirstOrDefaultAsync<Medicos>(
                "SELECT * FROM Medicos WHERE CRM = @CRM OR Email = @Email",
                new { CRM = medico.CRM, Email = medico.Email });

            if (medicoExistente != null)
            {
                response.Mensagem = "Médico com este CRM ou Email já existe.";
                response.Status = false;
                return response;
            }

            // Inserindo o médico no banco de dados
            var result = await _dbConnection.ExecuteAsync(
                "INSERT INTO Medicos (Nome, CRM, Email, Senha, Especialidade) VALUES (@Nome, @CRM, @Email, @Senha, @Especialidade)",
                medico);

            if (result == 0)
            {
                response.Mensagem = "Erro ao criar médico.";
                response.Status = false;
                return response;
            }

            // Retornando o DTO do médico criado
            response.Dados = _mapper.Map<MedicoListarDTO>(medico);
            response.Mensagem = "Médico criado com sucesso!";
            response.Status = true;
            return response;
        }



        public async Task<ResponseModel<List<MedicoListarDTO>>> EditarMedico(MedicoEditarDto medicoEditarDto)
        {
            var response = new ResponseModel<List<MedicoListarDTO>>();
            var medico = _mapper.Map<Medicos>(medicoEditarDto);
            var result = await _dbConnection.ExecuteAsync(
                "UPDATE Medicos SET Nome = @Nome, Especialidade = @Especialidade WHERE Id = @Id", medico);
            if (result == 0)
            {
                response.Mensagem = "Erro ao editar médico.";
                response.Status = false;
                return response;
            }
            return await BuscarMedicos();
        }

        public async Task<ResponseModel<List<MedicoListarDTO>>> RemoverMedico(int medicoId)
        {
            var response = new ResponseModel<List<MedicoListarDTO>>();
            var result = await _dbConnection.ExecuteAsync(
                "DELETE FROM Medicos WHERE Id = @Id", new { Id = medicoId });
            if (result == 0)
            {
                response.Mensagem = "Erro ao remover médico.";
                response.Status = false;
                return response;
            }
            return await BuscarMedicos();
        }

        public async Task<ResponseModel<MedicoListarDTO>> BuscarMedicoPorEmail(string email)
        {
            var response = new ResponseModel<MedicoListarDTO>();
            var medico = await _dbConnection.QueryFirstOrDefaultAsync<Medicos>(
                "SELECT * FROM Medicos WHERE Email = @Email", new { Email = email });

            if (medico == null)
            {
                response.Mensagem = "Médico não encontrado.";
                response.Status = false;
                return response;
            }
            Console.WriteLine($"Id do médico encontrado: {medico.Id}");


            response.Dados = _mapper.Map<MedicoListarDTO>(medico);
            response.Mensagem = "Médico encontrado com sucesso!";
            return response;
        }


        public async Task<ResponseModel<List<PacienteDTO>>> BuscarPacientesPorMedico(int medicoId)
        {
            var response = new ResponseModel<List<PacienteDTO>>();

            // 1. Consulta as consultas para o MedicoId
            var consultas = await _dbConnection.QueryAsync<Consulta>(
                "SELECT * FROM Consultas WHERE MedicoId = @MedicoId",
                new { MedicoId = medicoId });

            if (!consultas.Any())
            {
                response.Mensagem = "Nenhuma consulta encontrada para este médico.";
                response.Status = false;
                return response;
            }

            // 2. Extrai os IDs dos pacientes
            var pacienteIds = consultas.Select(c => c.PacienteId).Distinct().ToList();

            // 3. Consulta os pacientes pela lista de IDs
            var pacientes = await _dbConnection.QueryAsync<Pacientes>(
                "SELECT * FROM Pacientes WHERE Id IN @PacienteIds",
                new { PacienteIds = pacienteIds });

            var pacientesMapeados = _mapper.Map<List<PacienteDTO>>(pacientes.ToList());

            response.Dados = pacientesMapeados;
            response.Mensagem = "Pacientes encontrados com sucesso!";
            return response;
        }
    }
}
