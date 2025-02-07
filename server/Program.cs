using BowlingSimulator;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

BowlingGame game = new();

app.MapPost("/api/bowling", () =>
{
    game.SimulateGame();
    return Results.Ok(new
    {
        rolls = game.GetRolls(),
        score = game.CalculateScore()
    });
});



app.MapGet("/api/bowling", () =>
{
    return Results.Ok(new
    {
        Rolls = game.GetRolls(),
        score = game.CalculateScore()
    });
});


app.Run();