using GerenciarConsultas.Profiles;
using GerenciarConsultas.Repository;
using GerenciarConsultas.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Data.SqlClient;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Adicionando serviços ao container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuração do AutoMapper
builder.Services.AddAutoMapper(typeof(ProfileAutoMapper));

// Injeção de dependências (Interfaces e Implementações)
builder.Services.AddScoped<IPacienteInterface, PacienteService>();
builder.Services.AddScoped<IMedicoInterface, MedicoService>();
builder.Services.AddScoped<IConsultaInterface, ConsultaService>();
builder.Services.AddScoped<ConsultaService>();

// Configuração de conexão com o banco de dados (Dapper)
builder.Services.AddScoped<IDbConnection>(provider =>
    new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configuração do CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", policy =>
    {
        policy.WithOrigins("http://localhost:5173")  // Permitindo o front-end no localhost:5173
              .AllowAnyMethod()  // Permitindo qualquer método HTTP (GET, POST, PUT, DELETE, etc.)
              .AllowAnyHeader(); // Permitindo qualquer cabeçalho
    });
});

// Configuração da Autenticação JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var secretKey = builder.Configuration["Jwt:SecretKey"];

        // Adicionando log para verificar a chave secreta
        Console.WriteLine($"SecretKey: {secretKey}");

        if (string.IsNullOrEmpty(secretKey))
        {
            throw new ArgumentNullException(nameof(secretKey), "A chave secreta não pode ser nula ou vazia.");
        }

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };

        // Log para verificar configuração do JWT
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"Autenticação falhou: {context.Exception.Message}");
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                Console.WriteLine("Token validado com sucesso!");
                return Task.CompletedTask;
            }
        };
    });

// Configuração do arquivo de configuração
builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())  // Para garantir que o diretório correto seja usado
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

var app = builder.Build();

// Configuração do pipeline de requisições
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Habilitar CORS
app.UseCors("AllowLocalhost");  // Habilitando o CORS com a política configurada

// Habilitar autenticação e autorização
app.UseAuthentication();  // Habilitar autenticação
app.UseAuthorization();   // Habilitar autorização

// Mapear os controllers
app.MapControllers();

// Iniciar a aplicação
app.Run();
