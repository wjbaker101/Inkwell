using Core.Settings;
using Inkwell.Setup;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.AddSettings();
builder.Services.AddSingleton(builder.Configuration.Get<AppSecrets>()!);

builder.Services.AddControllers();
builder.Services.AddDependencies();

builder.Services.Configure<JsonOptions>(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();