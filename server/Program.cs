using BowlingSimulator;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

BowlingGame game = new BowlingGame();

app.MapGet("/api/bowling", () =>
{
    return Results.Ok(new
    {
        rolls = game.GetRolls(),
        score = game.CalculateScore()
    });
});

app.MapPost("/api/bowling/roll", () =>
{
    int pins = game.RollBall();
    return Results.Ok(new
    {
        pins,
        rolls = game.GetRolls(),
        score = game.CalculateScore()
    });
});

app.MapPost("/api/bowling/reset", () =>
{
    game.Reset();
    return Results.Ok(new { message = "Game reset" });
});

app.Run();
