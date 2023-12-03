using System;
using System.Diagnostics;
using VoiceAssistant.Controllers;
using VoiceAssistant.Data;
using VoiceAssistant.Helpers;
using VoiceAssistant.Models;

var builder = WebApplication.CreateBuilder(args);

var MyAllowSpecificOrigins = "ReactCors";

IConfiguration config = new ConfigurationBuilder()
           .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
           .Build();

var reactPath = config.GetValue<string>("ReactPath");
var reactUrl = config.GetValue<string>("ReactUrl");


builder.Services.AddHttpClient();

builder.Services.AddTransient<ICommandRepository, CommandRepository>();
builder.Services.AddTransient<IHomeCondition, HomeCondition>();
builder.Services.AddTransient<ICommandBuffer, CommandBuffer>();
builder.Services.AddTransient<CommandController>();
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseCors(x => x
    .AllowCredentials()
    .AllowAnyMethod()
    .AllowAnyHeader()
    .WithOrigins("http://localhost:34456"));

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

Process killNodePorts = new Process();
killNodePorts.StartInfo.FileName = "cmd.exe";
killNodePorts.StartInfo.WorkingDirectory = @$"{reactPath}";
killNodePorts.StartInfo.Arguments = "/C npm stop";

Task killNodePortsTask = new Task(() =>
{
    killNodePorts.Start();
    killNodePorts.WaitForExit();
}, TaskCreationOptions.LongRunning);
killNodePortsTask.Start();

Process reactProcess = new Process();
reactProcess.StartInfo.FileName = "cmd.exe";
reactProcess.StartInfo.WorkingDirectory = @$"{reactPath}";
reactProcess.StartInfo.Arguments = " /C npm start";
reactProcess.StartInfo.CreateNoWindow = true;
reactProcess.StartInfo.UseShellExecute = false;

ProcessStartInfo startInfo = new ProcessStartInfo("chrome.exe", reactUrl);
Task reactTask = new Task(() =>
{
    Process.Start(startInfo);
    reactProcess.Start();
    reactProcess.WaitForExit();
}, TaskCreationOptions.LongRunning);
reactTask.Start();

app.Run();