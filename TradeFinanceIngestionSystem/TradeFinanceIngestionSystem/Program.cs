using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TradeFinanceIngestionSystem.Application.Behaviors;
using TradeFinanceIngestionSystem.Application.Interfaces;
using TradeFinanceIngestionSystem.Infrastructure.DbContexts;
using TradeFinanceIngestionSystem.Infrastructure.Repositories;
using TradeFinanceIngestionSystem.Infrastructure.SeedData;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<NoteDataContext>(dbContextOptions => dbContextOptions.UseInMemoryDatabase(databaseName: "NoteDb"));
builder.Services.AddDbContext<InstrumentDataContext>(dbContextOptions => dbContextOptions.UseInMemoryDatabase(databaseName: "InstrumentDb"));
builder.Services.AddScoped<IInstrumentRepository, InstrumentRepository>();
builder.Services.AddScoped<INoteRepository, NoteRepository>();

// Register FluentValidation validators
builder.Services.AddValidatorsFromAssembly(typeof(TradeFinanceIngestionSystem.Application.Commands.CreateInstrument.CreateInstrumentCommand).Assembly);

// Register MediatR with validation behavior
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(TradeFinanceIngestionSystem.Application.Commands.CreateNote.CreateNoteCommand).Assembly);
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
});
var app = builder.Build();

// Seed the database
using (var scope = app.Services.CreateScope())
{
    var noteContext = scope.ServiceProvider.GetRequiredService<NoteDataContext>();
    var instrumentContext = scope.ServiceProvider.GetRequiredService<InstrumentDataContext>();
    DatabaseSeeder.SeedData(noteContext, instrumentContext);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
