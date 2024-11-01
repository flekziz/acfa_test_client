using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using src.app.module.Repositories;
using AutoMapper;
using src.app.module.Mapping;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<IConfigurationRepository, InMemoryConfigurationRepository>();
builder.Services.AddAutoMapper(typeof(MappingProfile));
var app = builder.Build();

app.MapControllers();
app.Run("http://localhost:5000");
