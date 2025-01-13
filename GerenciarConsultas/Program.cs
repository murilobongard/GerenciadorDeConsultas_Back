using System.Data;
using System.Data.SqlClient;
using GerenciarConsultas.Profiles; 
using GerenciarConsultas.Repository;
using GerenciarConsultas.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IPacienteInterface, PacienteService>();
builder.Services.AddAutoMapper(typeof(ProfileAutoMapper)); // Corrigido para o nome correto do Profile
builder.Services.AddScoped<IMedicoInterface, MedicoService>();
builder.Services.AddScoped<IConsultaInterface, ConsultaService>();
builder.Services.AddScoped<ConsultaService>();
builder.Services.AddScoped<IDbConnection>(provider =>
    new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
