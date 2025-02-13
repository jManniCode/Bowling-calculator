import { useState, useEffect } from "react";
import "./styles.css";

export default function ScoreBoard() {
    // State-variabler: rolls (21 element), scores (10 element), totalScore, gameOver och currentRoll.
    const [rolls, setRolls] = useState(Array(21).fill(null));
    const [scores, setScores] = useState(Array(10).fill("-"));
    const [totalScore, setTotalScore] = useState(0);
    const [gameOver, setGameOver] = useState(false);
    const [currentRoll, setCurrentRoll] = useState(0);
    // State för att trigga knappens animation
    const [animate, setAnimate] = useState(false);

    // När komponenten monteras, nollställ spelet.
    useEffect(() => {
        resetGame();
    }, []);

    // rollBall: Anropar backend för att simulera ett kast.
    // Backend returnerar ett objekt med updated rolls, scores och totalScore.
    const rollBall = async () => {
        if (gameOver) return;
        const res = await fetch("/api/bowling/roll", { method: "POST" });
        const data = await res.json();
        const pins = data.pins;

        const updatedRolls = [...rolls];
        let newRoll;
        if (currentRoll < 18 && currentRoll % 2 === 0 && pins === 10) {
            // Om det är en strike i rundor 1-9: spara kastet och markera nästa cell med "skip".
            updatedRolls[currentRoll] = pins;
            updatedRolls[currentRoll + 1] = "skip";
            newRoll = currentRoll + 2;
        } else {
            // Annars registrera kastet normalt.
            updatedRolls[currentRoll] = pins;
            newRoll = currentRoll + 1;
        }
        setRolls(updatedRolls);
        setScores(data.scores);
        setTotalScore(data.score);
        setCurrentRoll(newRoll);
        if (data.rolls.length >= 21) setGameOver(true);
    };

    //Triggar knappens animation och anropar rollBall.
    const handleRollClick = async () => {
        setAnimate(true);
        await rollBall();
        // Efter 200ms tas animationen bort.
        setTimeout(() => setAnimate(false), 200);
    };

    // Anropar backend för att återställa spelet och nollställer state.
    const resetGame = async () => {
        const res = await fetch("/api/bowling/reset", { method: "POST" });
        const data = await res.json();
        setRolls(data.rolls || Array(21).fill(null));
        setScores(data.scores || Array(10).fill("-"));
        setTotalScore(data.score || 0);
        setGameOver(false);
        setCurrentRoll(0);
    };

    return (
        <>
            <main>
                <header>
                    <h1>ScoreBoard</h1>
                </header>
                <section>
                    <section className="round-section">
                        {[...Array(9)].map((_, i) => (
                            <div className="round" id={`round${i + 1}`} key={i}>
                                <div className="points" id={`points${i + 1}-1`}>
                                    <p>
                                        {rolls[i * 2] == null
                                            ? "-"
                                            : rolls[i * 2] === 10
                                                ? "X"
                                                : rolls[i * 2]}
                                    </p>
                                </div>
                                <div className="points" id={`points${i + 1}-2`}>
                                    <p>
                                        {rolls[i * 2 + 1] === "skip"
                                            ? ""
                                            : rolls[i * 2 + 1] == null
                                                ? "-"
                                                : rolls[i * 2 + 1]}
                                    </p>
                                </div>
                            </div>
                        ))}
                        <div className="round" id="round10">
                            <div className="points" id="points10-1">
                                <p>{rolls[18] == null ? "-" : rolls[18] == 10 ? "X" : rolls[18]}</p>
                            </div>
                            <div className="points" id="points10-2">
                                <p>{rolls[19] == null ? "-" : rolls[19] == 10 ? "X" : rolls[19]}</p>
                            </div>
                            <div className="points" id="points10-3">
                                <p>{rolls[20] == null ? "-" : rolls[20] == 10 ? "X" : rolls[20]}</p>
                            </div>
                        </div>
                    </section>
                    <section className="score-section">
                        {[...Array(10)].map((_, i) => (
                            <div className="round-score" id={`score${i + 1}`} key={i}>
                                <p>{scores[i]}</p>
                            </div>
                        ))}
                    </section>
                </section>
            </main>
            <footer>
                <button
                    id="roll"
                    onClick={handleRollClick}
                    disabled={gameOver}
                    className={animate ? "button-animate" : ""}
                >
                    {gameOver ? "Game Over" : "Roll"}
                </button>
                <button onClick={resetGame}>New Game</button>
                <h2>Total: {totalScore}</h2>
            </footer>
        </>
    );
}
