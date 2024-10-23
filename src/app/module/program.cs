using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/api/configurations/{uid}", () => "Hello World!");
app.MapPost("/api/configurations/{uid}", () => "Hello World!");

app.MapGet("/api/configurations/{uid}/events", () => "Hello World!");

app.Run();

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
