using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build(); 

app.MapGet("/notes", async (AppDbContext db, string? search ) =>
{
    if (string.IsNullOrWhiteSpace(search))
    {
        List<NoteDto> x = await db.Notes.Select(b => new NoteDto(b.Id, b.Title, b.Content, b.CreatedAt)).ToListAsync();
        return Results.Ok(x);
    }
    else
    {
        List<NoteDto> x = await db.Notes.Where(b => b.Title.ToLower().Contains(search.ToLower())).Select(b => new NoteDto(b.Id,b.Title, b.Content, b.CreatedAt)).ToListAsync();
        return Results.Ok(x);
    }
});


app.MapGet("/notes/{id}", async (int id, AppDbContext db) =>
{
    Note? x = await db.Notes.FindAsync(id);
    if ( x is null) {return Results.NotFound();}
    else
    {
        NoteDto dto = new NoteDto(x.Id, x.Title, x.Content, x.CreatedAt);
        return Results.Ok(dto);
    }
});

app.MapPost("/notes", async (CreateNoteDto dto, AppDbContext db) =>
{
    if (string.IsNullOrWhiteSpace(dto.Title))
        return Results.BadRequest("Title обязателен");

    var note = new Note
    {
        Title = dto.Title,
        Content = dto.Content,
        CreatedAt = DateTime.UtcNow
    };
    db.Notes.Add(note);
    await db.SaveChangesAsync();

    var result = new NoteDto(note.Id, note.Title, note.Content, note.CreatedAt);
    return Results.Created($"/notes/{note.Id}", result);
});

app.MapPut("/notes/{id}", async (int id, CreateNoteDto input, AppDbContext db) =>
{   
    if (string.IsNullOrWhiteSpace(input.Title))
         return Results.BadRequest("Title обязателен");

    Note? note = await db.Notes.FindAsync(id);

    if (note is null) 
        return Results.NotFound();

    
    note.Title = input.Title;
    note.Content = input.Content;
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/notes/{id}", async (int id, AppDbContext db) =>
{
    var note = await db.Notes.FindAsync(id);

    if (note is null) return Results.NotFound();

    db.Notes.Remove(note);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

// app.MapGet("/test", async (AppDbContext db) =>
// {
//     // DbSet<Note> a = db.Notes;
//     // var b = db.Notes.Where(n => n.Id > 3);
//     // List<Note> c = await db.Notes.Where(n => n.Id > 3).ToListAsync();
//     // Note? d = await db.Notes.FirstOrDefaultAsync(n => n.Title == "zxc");
//     // int e = await db.Notes.CountAsync();
//     // bool f = await db.Notes.AnyAsync();
//     // var g = db.Notes.Select(n => n.Title);
//     // return Results.Ok();

//     var z = await db.Notes.Where(t => t.Title.Contains("te")).OrderByDescending(t => t.CreatedAt).Take(2).Select(t => t.Title).ToListAsync();
//     return Results.Ok(z);


// });

 
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

}
app.Run();
