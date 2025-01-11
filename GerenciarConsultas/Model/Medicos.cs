namespace GerenciarConsultas.Model
{
    public class Medicos
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public long? CRM { get; set; }
        public string? Email { get; set; }
        public string? Senha { get; set; }
        public string? Especialidade { get; set; }
    }
}
