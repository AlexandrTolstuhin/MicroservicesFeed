using MicroservicesFeed.Notifier.Services;
using MicroservicesFeed.Shared.Pulsar;
using MicroservicesFeed.Shared.Pulsar.Messaging;
using MicroservicesFeed.Shared.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddSerialization()
    .AddPulsar(builder.Configuration)
    .AddPulsarMessaging()
    .AddHostedService<NotifierMessagingBackgroundService>();

var app = builder.Build();

app.MapGet("/", () => "Microservices Feed Notifier");

app.Run();