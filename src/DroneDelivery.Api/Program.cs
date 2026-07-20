using DroneDelivery.Api.Storage;
using DroneDelivery.Api.Analysis;
using DroneDelivery.Api.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<InMemoryPlanningStore>();

builder.Services.Configure<FleetAnalysisOptions>(
    builder.Configuration.GetSection(
        "FleetAnalysis"));

builder.Services.AddSingleton<FleetAdvisor>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () =>
    Results.Redirect("/dashboard"));

app.MapGet("/dashboard", () =>
    Results.Redirect("/dashboard/index.html"));

app.Run();

public partial class Program;