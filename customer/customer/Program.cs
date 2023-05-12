using customer.Data;
using customer.Handlers;
using customer.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddSingleton<IHostedService, KafkaConsumerHandler>();
// Add services to the container.

builder.Services.AddControllers();
//builder.Services.AddSingleton<IConsumer,Consumer>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string connectionString = "server=db, 1433;User Id=sa ;database=offer;user=root;password=yasmeen@123;";
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString), ServiceLifetime.Singleton);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
