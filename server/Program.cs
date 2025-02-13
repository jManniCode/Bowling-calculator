using BowlingSimulator;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

BowlingGame game = new BowlingGame();

app.MapPost("/api/bowling/roll", () =>
{
    int pins = game.RollBall();
    return Results.Ok(new
    {
        pins,
        rolls = game.GetRolls(),
        scores = game.CalculateRoundScores(), // Returnerar uppdaterade rundpoäng
        score = game.CalculateScore()
    });
});

app.MapPost("/api/bowling/reset", () =>
{
    game.Reset();
    return Results.Ok(new
    {
        rolls = game.GetRolls(),
        scores = game.CalculateRoundScores(),
        score = game.CalculateScore()
    });
});

app.Run();
