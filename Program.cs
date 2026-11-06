using NotesApi.Endpoints;
using Microsoft.EntityFrameworkCore;
using NotesApi.Service;
using NotesApi.Middleware;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddScoped<INoteService,NoteService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build(); 

app.UseMiddleware<ErrorHandlingMiddleware>();

app.MapNoteEndpoints();
 
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

}

app.Run();
