using System;
using System.Collections.Generic;
using System.Linq;

namespace BowlingSimulator
{
    public class BowlingGame
    {
        // Lista som lagrar alla kast.
        private List<int> rolls = new List<int>();
        private Random random = new Random();

        // Lägger in ett kast i listan "rolls"
        public void Roll(int pins) => rolls.Add(pins);

        // Återställer spelet.
        public void Reset() => rolls.Clear();

        // RollBall simulerar ett kast och begränsar antalet käglor baserat på aktuell runda.
        public int RollBall()
        {
            int maxPins = 10;
            int ballCount = rolls.Count;
            int round = 0;
            int i = 0;

            // Bestäm vilken runda vi är i (runda 10 nehandlas separat).
            while (i < ballCount && round < 9)
            {
                if (rolls[i] == 10)
                {
                    // Strike: räknas som ett kast i rundan.
                    i++;
                    round++;
                }
                else
                {
                    if (i + 1 < ballCount)
                    {
                        // Normal runda: två kast.
                        i += 2;
                        round++;
                    }
                    else
                    {
                        // Rundan är ofullständig.
                        break;
                    }
                }
            }

            // För rundor 1–9: om vi redan har ett kast i den aktuella rundan, begränsa antalet käglor.
            if (round < 9)
            {
                if (i < ballCount)
                {
                    maxPins = 10 - rolls[i];
                }
                else
                {
                    maxPins = 10;
                }
            }
            else
            {
                // För 10:e rundan.
                int rollsIn10th = ballCount - i;
                if (rollsIn10th == 0)
                {
                    maxPins = 10;
                }
                else if (rollsIn10th == 1)
                {
                    int firstBall = rolls[i];
                    maxPins = firstBall == 10 ? 10 : 10 - firstBall;
                }
                else if (rollsIn10th == 2)
                {
                    int firstBall = rolls[i];
                    int secondBall = rolls[i + 1];
                    if (firstBall == 10)
                    {
                        maxPins = (secondBall == 10) ? 10 : 10 - secondBall;
                    }
                    else
                    {
                        maxPins = (firstBall + secondBall == 10) ? 10 : 0;
                    }
                }
                else
                {
                    maxPins = 0;
                }
            }

            // Generera ett slumpmässigt antal träffade käglor (mellan 0 och maxPins, inklusivt).
            int pins = random.Next(0, maxPins + 1);
            Roll(pins);
            return pins;
        }

        // Returnerar en kopia av rolls-listan.
        public List<int> GetRolls() => new List<int>(rolls);

        // CalculateScore beräknar den totala poängen enligt standard bowlingregler.
        public int CalculateScore()
        {
            int score = 0;
            int rollIndex = 0;
            for (int round = 0; round < 10; round++)
            {
                if (rollIndex >= rolls.Count)
                    break;

                if (round < 9)
                {
                    if (rolls[rollIndex] == 10)
                    {
                        score += 10 + GetRoll(rollIndex + 1) + GetRoll(rollIndex + 2);
                        rollIndex++;
                    }
                    else if (rolls[rollIndex] + GetRoll(rollIndex + 1) == 10)
                    {
                        score += 10 + GetRoll(rollIndex + 2);
                        rollIndex += 2;
                    }
                    else
                    {
                        score += rolls[rollIndex] + GetRoll(rollIndex + 1);
                        rollIndex += 2;
                    }
                }
                else
                {
                    while (rollIndex < rolls.Count)
                    {
                        score += rolls[rollIndex++];
                    }
                }
            }
            return score;
        }

        // CalculateRoundScores beräknar en array med kumulativa poäng per runda (10 element).
        public List<int> CalculateRoundScores()
        {
            List<int> roundScores = new List<int>(new int[10]); // Skapar en lista med 10 nollor.
            int score = 0;
            int rollIndex = 0;
            for (int round = 0; round < 10; round++)
            {
                if (rollIndex >= rolls.Count)
                    break;

                if (round < 9)
                {
                    if (rolls[rollIndex] == 10)
                    {
                        score += 10 + GetRoll(rollIndex + 1) + GetRoll(rollIndex + 2);
                        roundScores[round] = score;
                        rollIndex++;
                    }
                    else if (rolls[rollIndex] + GetRoll(rollIndex + 1) == 10)
                    {
                        score += 10 + GetRoll(rollIndex + 2);
                        roundScores[round] = score;
                        rollIndex += 2;
                    }
                    else
                    {
                        score += rolls[rollIndex] + GetRoll(rollIndex + 1);
                        roundScores[round] = score;
                        rollIndex += 2;
                    }
                }
                else
                {
                    while (rollIndex < rolls.Count)
                    {
                        score += rolls[rollIndex++];
                    }
                    roundScores[round] = score;
                }
            }
            return roundScores;
        }

        // Hjälpfunktion som returnerar värdet för ett kast vid ett givet index, eller 0 om indexet är utanför listan.
        private int GetRoll(int index)
        {
            return index < rolls.Count ? rolls[index] : 0;
        }
    }
}
