namespace GerenciarConsultas.DTO
{
    public class ConsultaDTO
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public int MedicoId { get; set; }
        public int PacienteId { get; set; }
        public string? Observacoes { get; set; }
        public decimal ValorConsulta { get; set; }
        public string? TipoConsulta { get; set; }
    }
}
