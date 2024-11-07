using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//var summaries = new[]
//{
//    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
//};

//app.MapGet("/weatherforecast", () =>
//{
//    var forecast = Enumerable.Range(1, 5).Select(index =>
//        new WeatherForecast
//        (
//            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//            Random.Shared.Next(-20, 55),
//            summaries[Random.Shared.Next(summaries.Length)]
//        ))
//        .ToArray();
//    return forecast;
//})
//.WithName("GetWeatherForecast")
//.WithOpenApi();
app.MapGet("/getSnakes", () =>
{
    string folderPath = "Data/Snakes.json";
    var jsonStr=File.ReadAllText(folderPath);
    var result=JsonConvert.DeserializeObject<SnakeResponseModel>(jsonStr);
    return Results.Ok(result!.Tbl_Snakes);
});

app.MapGet("/getSnake/{id}", (int id) =>
{
    string folderPath = "Data/Snakes.json";
    var jsonStr = File.ReadAllText(folderPath);
    var result = JsonConvert.DeserializeObject<SnakeResponseModel>(jsonStr);

    if(result is null)
        return Results.NotFound("No Record Found");
    return Results.Ok(result!.Tbl_Snakes.FirstOrDefault(x=>x.Id==id));
});
app.Run();



public class SnakeResponseModel
{
    public Tbl_Snakes[] Tbl_Snakes { get; set; }
}

public class Tbl_Snakes
{
    public int Id { get; set; }
    public string MMName { get; set; }
    public string EngName { get; set; }
    public string Detail { get; set; }
    public string IsPoison { get; set; }
    public string IsDanger { get; set; }
}