import { useState, useEffect } from "react";
import "./styles.css";

export default function ScoreBoard() {
    const [rolls, setRolls] = useState(Array(21).fill(null));
    const [scores, setScores] = useState(Array(10).fill("-"));
    const [currentRoll, setCurrentRoll] = useState(0);
    const [totalScore, setTotalScore] = useState(0);
    const [gameOver, setGameOver] = useState(false);

    useEffect(() => {
        resetGame();
    }, []);

    const rollBall = async () => {
        if (gameOver) return;
   
        const res = await fetch("/api/bowling/roll", { method: "POST" });
        const data = await res.json();
        const pins = data.pins;

        const updatedRolls = [...rolls];

        if (currentRoll < 18 && currentRoll % 2 === 0 && pins === 10) {
            updatedRolls[currentRoll] = pins;
            updatedRolls[currentRoll + 1] = "skip";
            setRolls(updatedRolls);
            updateScores(updatedRolls);
            setCurrentRoll(currentRoll + 2);
        } else {
            updatedRolls[currentRoll] = pins;
            setRolls(updatedRolls);
            updateScores(updatedRolls);
            setCurrentRoll(currentRoll + 1);
        }

        if (currentRoll >= 20) {
            setGameOver(true);
        }
    };

    const updateScores = (rollsArr) => {
        let score = 0;
        let rollIndex = 0;
        let newScores = Array(10).fill("-");

        
        for (let round = 0; round < 9; round++) {
       
            while (rollIndex < rollsArr.length && rollsArr[rollIndex] === "skip") {
                rollIndex++;
            }
            if (rollIndex >= rollsArr.length || rollsArr[rollIndex] === null) break;

            let firstRoll = rollsArr[rollIndex];
            if (firstRoll === 10) {
              
                let bonus1 = null, bonus2 = null;
          
                let i = rollIndex + 1;
                while (i < rollsArr.length && (rollsArr[i] === null || rollsArr[i] === "skip")) {
                    i++;
                }
                if (i < rollsArr.length) bonus1 = rollsArr[i];

               
                i++;
                while (i < rollsArr.length && (rollsArr[i] === null || rollsArr[i] === "skip")) {
                    i++;
                }
                if (i < rollsArr.length) bonus2 = rollsArr[i];

                if (bonus1 !== null && bonus2 !== null) {
                    score += 10 + bonus1 + bonus2;
                    newScores[round] = score;
                }
                rollIndex++;
            } else {
               
                let secondRoll = rollsArr[rollIndex + 1];
                if (secondRoll === "skip") {
                    rollIndex += 2;
                    continue;
                }
                if (secondRoll === null || secondRoll === undefined) break;
                let frameScore = firstRoll + secondRoll;
                if (frameScore === 10) {
                
                    let bonus = null;
                    let i = rollIndex + 2;
                    while (i < rollsArr.length && (rollsArr[i] === null || rollsArr[i] === "skip")) {
                        i++;
                    }
                    if (i < rollsArr.length) bonus = rollsArr[i];
                    if (bonus !== null) {
                        score += 10 + bonus;
                        newScores[round] = score;
                    }
                } else {
                    score += frameScore;
                    newScores[round] = score;
                }
                rollIndex += 2;
            }
        }

       
        let round10Rolls = [];
        for (let i = rollIndex; i < rollsArr.length; i++) {
            if (rollsArr[i] !== null && rollsArr[i] !== "skip") {
                round10Rolls.push(rollsArr[i]);
            }
        }
        if (round10Rolls.length >= 2) {
            let tenthScore = round10Rolls[0] + round10Rolls[1];
            if (
                (round10Rolls[0] === 10 || round10Rolls[0] + round10Rolls[1] === 10) &&
                round10Rolls.length >= 3
            ) {
                tenthScore += round10Rolls[2];
            }
            score += tenthScore;
            newScores[9] = score;
        }
        setScores(newScores);
        setTotalScore(score);
    };


    const resetGame = async () => {
        await fetch("/api/bowling/reset", { method: "POST" });
        setRolls(Array(21).fill(null));
        setScores(Array(10).fill("-"));
        setCurrentRoll(0);
        setTotalScore(0);
        setGameOver(false);
    };

    return (<>
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
                                    {rolls[i * 2] !== null
                                        ? rolls[i * 2] === 10
                                            ? "X"
                                            : rolls[i * 2]
                                        : "-"}
                                </p>
                            </div>
                            <div className="points" id={`points${i + 1}-2`}>
                                <p>
                                    {rolls[i * 2 + 1] !== "skip"
                                        ? rolls[i * 2 + 1] !== null
                                            ? rolls[i * 2 + 1]
                                            : "-"
                                        : ""}
                                </p>
                            </div>
                        </div>
                    ))}
                    <div className="round" id="round10">
                        <div className="points" id="points10-1">
                            <p>{rolls[18] !== null ? rolls[18] : "-"}</p>
                        </div>
                        <div className="points" id="points10-2">
                            <p>{rolls[19] !== null ? rolls[19] : "-"}</p>
                        </div>
                        <div className="points" id="points10-3">
                            <p>{rolls[20] !== null ? rolls[20] : "-"}</p>
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
            <button onClick={rollBall} disabled={gameOver}>
                {gameOver ? "Game Over" : "Roll"}
            </button>
            <button onClick={resetGame}>New Game</button>
            <h2>Total: {totalScore}</h2>
                
        </footer>
        </>
    );
}
