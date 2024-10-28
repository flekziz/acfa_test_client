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

//"    "configuration": {
//        "opaque_params": [],
//        "properties": [
//            {
//                "id": "ip",
//                "value_string": "10.0.11.217"
//            },
//            {
//                "id": "login",
//                "value_string": "Administrator"
//            },
//            {
//                "id": "password",
//                "value_string": "Isb2024"
//            },
//            {
//                "id": "db_password",
//                "value_string": "12345"
//            }
//        ],
//        "traits": [],
//        "type": "SPHINX_SRV",
//        "uid": "ACFA.1"
//    }"
