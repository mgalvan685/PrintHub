using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PrintHub.Database;
using PrintHub.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<PrinterService>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Validators in the PrintHub project
// NOTE: You do not need to add new ones here. As long as they inherit from AbstractValidator<T>, they will be automatically registered, as long as they are in
// the same assembly as this Program.cs file. If you create a new validator in a different assembly, you will need to add it here.
builder.Services.AddValidatorsFromAssemblyContaining<NewPrinterDtoValidator>();

builder.Services.AddDbContext<PrintHubContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PrintHub")));


var app = builder.Build();

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
