namespace GerenciarConsultas.DTO
{
    public class EditarPacienteDto
    {
      
        public string? Nome { get; set; }
        public DateTime? DataNascimento { get; set; }
        public long? Telefone { get; set; }
        public long? CPF { get; set; }
        public string? Email { get; set; }
    }
}
