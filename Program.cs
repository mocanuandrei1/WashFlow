using Microsoft.EntityFrameworkCore;
using WashFlow.Api.Data;
using WashFlow.Api.Services.Implementations;
using WashFlow.Api.Services.Interfaces;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(opt =>
        opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())
    );

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<WashFlowDbContext>(options =>
    options.UseSqlite("Data Source=/data/washflow.db"));

builder.Services.AddScoped<IStationService, StationService>();
builder.Services.AddScoped<IMaintenanceService, MaintenanceService>();
builder.Services.AddScoped<IProgramService, ProgramService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<WashFlowDbContext>();
    db.Database.Migrate();
}


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.MapControllers();
app.Run();
