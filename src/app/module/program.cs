using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using app.module.Services.BackgroungTasks;
using Grpc.Net.Client;
using repository.module.Models.Internal;
using repository.module.Implementations;
using repository.module.Profiles;
using repository.module;
using System;
using repository.module.Interfaces;
using repository.module.Models;


var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.AddControllers();

builder.Services.AddRepository(configuration);

builder.Services.AddAutoMapper(typeof(ConfigurationMappingProfile).Assembly);
builder.Services.AddLogging();


builder.Services.AddHostedService<GrpcStreamWorker>();
var app = builder.Build();

app.MapControllers();
app.Run("http://localhost:5000");
