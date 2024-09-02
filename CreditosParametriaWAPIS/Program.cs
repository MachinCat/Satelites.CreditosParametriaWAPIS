using CreditosParametriaAD;
using CreditosParametriaEN.DTOs;
using CreditosParametriaRN;
using CreditosParametriaRN.Services;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuraci�n de la cadena de conexi�n (reemplaza con tu cadena de conexi�n real)
string connectionString = builder.Configuration.GetConnectionString("Satel");

// Registrar dependencias usando la inyecci�n de dependencias
builder.Services.AddTransient<ModalidadRepository>(provider => new ModalidadRepository(connectionString));
builder.Services.AddTransient<EvaluacionRepository>(provider => new EvaluacionRepository(connectionString));


builder.Services.AddTransient<ModalidadServices>();
builder.Services.AddTransient<EvaluacionService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
