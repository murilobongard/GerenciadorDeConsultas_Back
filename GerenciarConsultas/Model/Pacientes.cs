namespace GerenciarConsultas.Model
{
    public class Pacientes
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public DateTime? DataNascimento { get; set; }  
        public long? Telefone { get; set; }
        public long? CPF { get; set; }
        public string? Email { get; set; }
        public string? Senha { get; set; }
    }
}
