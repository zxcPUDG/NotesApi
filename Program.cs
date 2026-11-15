using NotesApi.Endpoints;
using Microsoft.EntityFrameworkCore;
using NotesApi.Services;
using NotesApi.Middleware;
using FluentValidation;
using NotesApi.Validators;
using Serilog;
using NotesApi.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSerilog((services, lc) => lc
    .ReadFrom.Configuration(builder.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext());

builder.Services
    .AddOptions<NoteLimitsOptions>()
    .BindConfiguration("NoteLimits")
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<INoteService, NoteService>();

builder.Services.AddValidatorsFromAssemblyContaining<CreateNoteDtoValidator>();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();


app.UseSerilogRequestLogging();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.MapNoteEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

}

app.Run();
