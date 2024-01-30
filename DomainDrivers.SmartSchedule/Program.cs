using System.Text.Json;
using DomainDrivers.SmartSchedule;
using DomainDrivers.SmartSchedule.Availability;
using DomainDrivers.SmartSchedule.Planning;
using DomainDrivers.SmartSchedule.Shared;
using Microsoft.EntityFrameworkCore;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Postgres");
var dataSource = new NpgsqlDataSourceBuilder(connectionString)
    .ConfigureJsonOptions(new JsonSerializerOptions() { IgnoreReadOnlyProperties = true, IgnoreReadOnlyFields = true})
    .EnableDynamicJson()
    .Build();
builder.Services.AddDbContext<SmartScheduleDbContext>(options => { options.UseNpgsql(dataSource); });
builder.Services.AddShared();
builder.Services.AddPlanning();
builder.Services.AddAvailability();

var app = builder.Build();

app.Run();

public partial class Program;