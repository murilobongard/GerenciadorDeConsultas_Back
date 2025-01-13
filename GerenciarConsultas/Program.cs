using GerenciarConsultas.Profiles;
using GerenciarConsultas.Repository;
using GerenciarConsultas.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Data.SqlClient;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Adicionando servi�os ao container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configura��o do AutoMapper
builder.Services.AddAutoMapper(typeof(ProfileAutoMapper));

// Inje��o de depend�ncias (Interfaces e Implementa��es)
builder.Services.AddScoped<IPacienteInterface, PacienteService>();
builder.Services.AddScoped<IMedicoInterface, MedicoService>();
builder.Services.AddScoped<IConsultaInterface, ConsultaService>();
builder.Services.AddScoped<ConsultaService>();

// Configura��o de conex�o com o banco de dados (Dapper)
builder.Services.AddScoped<IDbConnection>(provider =>
    new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configura��o do CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", policy =>
    {
        policy.WithOrigins("http://localhost:5173")  // Permitindo o front-end no localhost:5173
              .AllowAnyMethod()  // Permitindo qualquer m�todo HTTP (GET, POST, PUT, DELETE, etc.)
              .AllowAnyHeader(); // Permitindo qualquer cabe�alho
    });
});

// Configura��o da Autentica��o JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var secretKey = builder.Configuration["Jwt:SecretKey"];

        // Adicionando log para verificar a chave secreta
        Console.WriteLine($"SecretKey: {secretKey}");

        if (string.IsNullOrEmpty(secretKey))
        {
            throw new ArgumentNullException(nameof(secretKey), "A chave secreta n�o pode ser nula ou vazia.");
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

        // Log para verificar configura��o do JWT
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"Autentica��o falhou: {context.Exception.Message}");
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                Console.WriteLine("Token validado com sucesso!");
                return Task.CompletedTask;
            }
        };
    });

// Configura��o do arquivo de configura��o
builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())  // Para garantir que o diret�rio correto seja usado
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

var app = builder.Build();

// Configura��o do pipeline de requisi��es
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Habilitar CORS
app.UseCors("AllowLocalhost");  // Habilitando o CORS com a pol�tica configurada

// Habilitar autentica��o e autoriza��o
app.UseAuthentication();  // Habilitar autentica��o
app.UseAuthorization();   // Habilitar autoriza��o

// Mapear os controllers
app.MapControllers();

// Iniciar a aplica��o
app.Run();
