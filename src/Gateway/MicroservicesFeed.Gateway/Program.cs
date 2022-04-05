var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("YARP"));

var app = builder.Build();

app.MapGet("/", () => "Microservices Feed Gateway");
app.MapReverseProxy();

app.Run();