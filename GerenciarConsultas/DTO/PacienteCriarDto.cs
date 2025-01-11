namespace GerenciarConsultas.DTO
{
    public class PacienteCriarDto
    {
        public string Nome { get; set; } = null!;
        public DateTime DataNascimento { get; set; }
        public long? Telefone { get; set; }
        public long? CPF { get; set; }
        public string Email { get; set; } = null!;
        public string Senha { get; set; } = null!;
    }
}
