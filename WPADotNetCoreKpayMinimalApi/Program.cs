using WPADotNetCore.Databases.Models;
using WPADotNetCore.Domain.Kpay;
using WPADotNetCore.Databases.ViewModels;
using WPADotNetCore.KpayMinimalApi.EndPoints.Kpay;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<KpayService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapKpayEndPoint();
app.Run();

