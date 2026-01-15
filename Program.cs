using Microsoft.EntityFrameworkCore;
using WashFlow.Api.Data;
using WashFlow.Api.Services.Implementations;
using WashFlow.Api.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<WashFlowDbContext>(options =>
    options.UseSqlite("Data Source=washflow.db"));

builder.Services.AddScoped<IStationService, StationService>();
builder.Services.AddScoped<IMaintenanceService, MaintenanceService>();
builder.Services.AddScoped<IProgramService, ProgramService>();
builder.Services.AddScoped<IReportService, ReportService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
