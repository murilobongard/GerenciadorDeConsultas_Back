namespace GerenciarConsultas.DTO
{
    public class MedicoListarDTO
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public long? CRM { get; set; }
        public string? Email { get; set; }   
        public string? Especialidade { get; set; }
    }
}
