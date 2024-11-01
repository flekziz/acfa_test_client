using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using src.app.module.Repositories;
using src.app.module.Mapping;
using src.app.module.Services.BackgroungTasks;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<IConfigurationRepository, InMemoryConfigurationRepository>();
builder.Services.AddAutoMapper(typeof(MappingProfile));


//регистрация grpc клиента
//..//

builder.Services.AddHostedService<GrpcStreamWorker>();
var app = builder.Build();

app.MapControllers();
app.Run("http://localhost:5000");
