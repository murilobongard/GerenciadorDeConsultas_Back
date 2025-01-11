namespace GerenciarConsultas.DTO
{
    public class PacienteDTO
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Data { get; set; }
        public long? Telefone { get; set; }  
        public string? Email { get; set; }
    }
}
