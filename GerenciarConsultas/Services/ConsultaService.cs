using Dapper;
using System.Data;
using GerenciarConsultas.Model;
using GerenciarConsultas.Services;
using GerenciarConsultas.DTO;

namespace GerenciarConsultas.Repository
{
    public class ConsultaService : IConsultaInterface
    {
        private readonly IDbConnection _dbConnection;

        public ConsultaService(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<ResponseModel<List<ConsultaListaDT0>>> BuscarConsultas()
        {
            ResponseModel<List<ConsultaListaDT0>> response = new ResponseModel<List<ConsultaListaDT0>>();

            var consultasBanco = await _dbConnection.QueryAsync<Consulta>("SELECT * FROM Consultas");

            if (!consultasBanco.Any())
            {
                response.Mensagem = "Nenhuma consulta localizada.";
                response.Status = false;
                return response;
            }

            // Garantir que o mapeamento está correto
            response.Dados = consultasBanco.Select(c => new ConsultaListaDT0
            {
                Id = c.Id,
                Data = c.Data,
                ValorConsulta = c.ValorConsulta,
                MedicoId = c.MedicoId,
                PacienteId = c.PacienteId,
                Observacoes = c.Observacoes, // Incluir observações para mapear corretamente
                TipoConsulta = c.TipoConsulta // Incluir tipo de consulta
            }).ToList();

            response.Mensagem = "Consultas localizadas com sucesso!";
            response.Status = true;
            return response;
        }



        public async Task<ResponseModel<ConsultaListaDT0>> BuscarConsultaPorId(int consultaId)
        {
            ResponseModel<ConsultaListaDT0> response = new ResponseModel<ConsultaListaDT0>();

            var consultaBanco = await _dbConnection.QueryFirstOrDefaultAsync<Consulta>(
                 "SELECT Id, Data, ValorConsulta, MedicoId, PacienteId, Observacoes, TipoConsulta " +
                 "FROM Consultas WHERE Id = @Id",
                 new { Id = consultaId }
);
            if (consultaBanco == null)
            {
                response.Mensagem = "Consulta não encontrada.";
                response.Status = false;
                return response;
            }

            response.Dados = new ConsultaListaDT0
            {
                Id = consultaBanco.Id,
                Data = consultaBanco.Data,
                ValorConsulta = consultaBanco.ValorConsulta,
                MedicoId = consultaBanco.MedicoId,
                PacienteId = consultaBanco.PacienteId,
                Observacoes = consultaBanco.Observacoes, 
                TipoConsulta = consultaBanco.TipoConsulta 
            };

            response.Mensagem = "Consulta localizada com sucesso!";
            response.Status = true;
            return response;
        }


        public async Task<ResponseModel<Consulta>> EditarConsulta(int id, ConsultaEditarDto consultaEditarDto)
        {
            ResponseModel<Consulta> response = new ResponseModel<Consulta>();

            // Verifica se o médico existe
            var medicoExiste = await _dbConnection.ExecuteScalarAsync<bool>(
                "SELECT COUNT(1) FROM Medicos WHERE Id = @MedicoId",
                new { consultaEditarDto.MedicoId });

            if (!medicoExiste)
            {
                response.Mensagem = "O médico especificado não existe.";
                response.Status = false;
                return response;
            }

            // Verifica se o paciente existe
            var pacienteExiste = await _dbConnection.ExecuteScalarAsync<bool>(
                "SELECT COUNT(1) FROM Pacientes WHERE Id = @PacienteId",
                new { consultaEditarDto.PacienteId });

            if (!pacienteExiste)
            {
                response.Mensagem = "O paciente especificado não existe.";
                response.Status = false;
                return response;
            }

            // Atualiza a consulta no banco de dados
            var consultaAtualizada = await _dbConnection.ExecuteAsync(
                "UPDATE Consultas SET Data = @Data, ValorConsulta = @ValorConsulta, MedicoId = @MedicoId, PacienteId = @PacienteId, Observacoes = @Observacoes, TipoConsulta = @TipoConsulta WHERE Id = @Id",
                new
                {
                    Id = id,
                    consultaEditarDto.Data,
                    consultaEditarDto.ValorConsulta,
                    consultaEditarDto.MedicoId,
                    consultaEditarDto.PacienteId,
                    consultaEditarDto.Observacoes,
                    consultaEditarDto.TipoConsulta
                });

            if (consultaAtualizada == 0)
            {
                response.Mensagem = "Consulta não encontrada ou erro ao editar consulta.";
                response.Status = false;
                return response;
            }

            // Busca a consulta atualizada para retornar
            var consulta = await _dbConnection.QueryFirstOrDefaultAsync<Consulta>(
                "SELECT * FROM Consultas WHERE Id = @Id", new { Id = id });

            if (consulta == null)
            {
                response.Mensagem = "Consulta não encontrada após atualização.";
                response.Status = false;
                return response;
            }

            response.Dados = consulta;
            response.Mensagem = "Consulta atualizada com sucesso!";
            response.Status = true;
            return response;
        }





        public async Task<ResponseModel<List<ConsultaListaDT0>>> RemoverConsulta(int consultaId)
        {
            ResponseModel<List<ConsultaListaDT0>> response = new ResponseModel<List<ConsultaListaDT0>>();

            var consultaRemovida = await _dbConnection.ExecuteAsync(
                "DELETE FROM Consultas WHERE Id = @Id", new { Id = consultaId });

            if (consultaRemovida == 0)
            {
                response.Mensagem = "Erro ao remover consulta.";
                response.Status = false;
                return response;
            }

            var consultas = await BuscarConsultas();

            response.Dados = consultas.Dados;
            response.Mensagem = "Consulta removida com sucesso!";
            response.Status = true;
            return response;
        }

        public async Task<ResponseModel<List<ConsultaListaDT0>>> CriarConsulta(ConsultaCriarDtos consultaCriarDto)
        {
            ResponseModel<List<ConsultaListaDT0>> response = new ResponseModel<List<ConsultaListaDT0>>();

            var medicoExistente = await _dbConnection.QueryFirstOrDefaultAsync<Medicos>(
                "SELECT * FROM Medicos WHERE Id = @MedicoId", new { MedicoId = consultaCriarDto.MedicoId });

            if (medicoExistente == null)
            {
                response.Mensagem = "Médico não encontrado.";
                response.Status = false;
                return response;
            }

            var pacienteExistente = await _dbConnection.QueryFirstOrDefaultAsync<Pacientes>(
                "SELECT * FROM Pacientes WHERE Id = @PacienteId", new { PacienteId = consultaCriarDto.PacienteId });

            if (pacienteExistente == null)
            {
                response.Mensagem = "Paciente não encontrado.";
                response.Status = false;
                return response;
            }

            // Inserir a consulta e capturar o ID gerado
            var consultaId = await _dbConnection.QuerySingleAsync<int>(
                @"INSERT INTO Consultas (Data, ValorConsulta, MedicoId, PacienteId, Observacoes, TipoConsulta) 
        VALUES (@Data, @ValorConsulta, @MedicoId, @PacienteId, @Observacoes, @TipoConsulta);
        SELECT CAST(SCOPE_IDENTITY() AS INT);", consultaCriarDto);

            if (consultaId == 0)
            {
                response.Mensagem = "Erro ao criar consulta.";
                response.Status = false;
                return response;
            }

            // Recupera a consulta criada com o novo ID
            var consultaCriada = await _dbConnection.QueryFirstOrDefaultAsync<Consulta>(
                "SELECT * FROM Consultas WHERE Id = @Id", new { Id = consultaId });

            if (consultaCriada == null)
            {
                response.Mensagem = "Erro ao recuperar consulta criada.";
                response.Status = false;
                return response;
            }

            // Mapeia corretamente para o DTO
            var consultaDTO = new ConsultaListaDT0
            {
                Id = consultaCriada.Id,
                Data = consultaCriada.Data,
                ValorConsulta = consultaCriada.ValorConsulta,
                MedicoId = consultaCriada.MedicoId,
                PacienteId = consultaCriada.PacienteId,
                Observacoes = consultaCriada.Observacoes,
                TipoConsulta = consultaCriada.TipoConsulta
            };

            response.Dados = new List<ConsultaListaDT0> { consultaDTO };
            response.Mensagem = "Consulta criada com sucesso!";
            response.Status = true;
            return response;
        }



    }
}
