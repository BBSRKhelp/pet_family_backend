using PetFamily.File.Presentation;
using PetFamily.Species.Presentation;
using PetFamily.Volunteer.Presentation;
using PetFamily.Web;
using PetFamily.Web.Middlewares;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddWeb(builder.Configuration)
    .AddFile(builder.Configuration)
    .AddSpecies(builder.Configuration)
    .AddVolunteer(builder.Configuration);

var app = builder.Build();

app.UseExceptionMiddleware();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

namespace PetFamily.Web
{
    public partial class Program;
}