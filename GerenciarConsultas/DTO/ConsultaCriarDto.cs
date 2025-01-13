namespace GerenciarConsultas.DTO
{
    public class ConsultaCriarDtos
    {

        public DateTime Data { get; set; }
        public decimal ValorConsulta { get; set; }
        public int MedicoId { get; set; }
        public int PacienteId { get; set; }
        public string Observacoes { get; set; } = string.Empty; 
        public string TipoConsulta { get; set; } = string.Empty;
    }
}
