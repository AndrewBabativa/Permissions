using Microsoft.EntityFrameworkCore;
using Permissions.Infrastructure;
using Microsoft.OpenApi.Models;
using MediatR;
using Permissions.Application.Handlers;
using Permissions.Domain.Dtos;
using Permissions.Infrastructure.Queries;
using Permissions.Infrastructure.Commands;
using Nest;
using Permissions.Domain.Entities;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddSingleton(new System.Uri("http://localhost:9200"));

builder.Services.AddSingleton<KafkaProducer>();

// Configuración de la conexión a Elasticsearch
var settings = new ConnectionSettings(new Uri("http://localhost:9200")).DefaultIndex("permissions-log");
var client = new ElasticClient(settings);
builder.Services.AddSingleton(client);

builder.Services.AddSingleton(typeof(IElasticsearchService<>), typeof(ElasticsearchService<>));
builder.Services.AddScoped<ElasticsearchService<Permission>>();
builder.Services.AddScoped<ElasticsearchService<PermissionType>>();
builder.Services.AddScoped<UnitOfWork>();

// Configuración de la cadena de conexión
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Configuración de Entity Framework
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//Configuraciòn Serilog
builder.Host.UseSerilog((context, configuration) =>
{
  configuration.ReadFrom.Configuration(context.Configuration);
});

// Configuración de MediatR
builder.Services.AddMediatR(typeof(Program).Assembly);

// Registra los manejadores de comandos y consultas
builder.Services.AddTransient<IRequestHandler<GetPermissionsQuery, IEnumerable<PermissionDto>>, GetPermissionHandler>();
builder.Services.AddTransient<IRequestHandler<ModifyPermissionCommand, PermissionDto>, ModifyPermissionHandler>();
builder.Services.AddTransient<IRequestHandler<RequestPermissionCommand, PermissionDto>, RequestPermissionHandler>();

builder.Services.AddTransient<IRequestHandler<GetPermissionTypeQuery, IEnumerable<PermissionTypeDto>>,GetPermissionTypeHandler>();
builder.Services.AddTransient<IRequestHandler<CreatePermissionTypeCommand, PermissionTypeDto>, CreatePermissionTypeHandler>();

// Configuración de Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
  c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});


// Otros serviciosDomain.ApplicationDbContext
builder.Services.AddControllers();

var app = builder.Build();
app.UseSerilogRequestLogging();
if (app.Environment.IsDevelopment())
{
  app.UseDeveloperExceptionPage();
  app.UseSwagger();
  app.UseSwaggerUI(c =>
  {
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Andres Babativa Goyeneche");
  });
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
