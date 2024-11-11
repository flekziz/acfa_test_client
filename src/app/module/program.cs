using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using src.app.module.Services.BackgroungTasks;
using Grpc.Net.Client;
using repository.module.Models.Internal;
using repository.module.Implementations;
using repository.module.Profiles;
using System;
using repository.module.Interfaces;
using repository.module.Models;


var builder = WebApplication.CreateBuilder(args);

//экстеншн метод для регистрации
//builder.Services.AddConfigurationServices();

builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


//регистрация grpc клиента
//..//

builder.Services.AddHostedService<GrpcStreamWorker>();
var app = builder.Build();

app.MapControllers();
app.Run("http://localhost:5000");
