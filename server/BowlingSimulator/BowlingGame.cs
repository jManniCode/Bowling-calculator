using System;
using System.Collections.Generic;

namespace BowlingSimulator
{
    public class BowlingGame
    {
        private List<int> rolls = new List<int>();
        private Random random = new();
        public void Roll(int pins)
        {
            rolls.Add(pins);
        }

        public List<int> GetRolls()
        {
            return rolls;
        }

        public int CalculateScore()
        {
            int score = 0;
            int rollIndex = 0;

            for (int round = 0; round < 10; round++)
            {
                if (rolls[rollIndex] == 10)
                {
                    score += 10 + rolls[rollIndex + 1] + rolls[rollIndex + 2];
                    rollIndex++;
                }
                else if (rolls[rollIndex] + rolls[rollIndex + 1] == 10)
                {
                    score += 10 + rolls[rollIndex + 2];
                    rollIndex += 2;
                }
                else
                {
                    score += rolls[rollIndex] + rolls[rollIndex + 1];
                    rollIndex += 2;
                }
            }
            return score;
        }

        public void SimulateGame()
        {
            rolls.Clear();

            for (int round = 0; round < 9; round++)
            {
                int firstRoll = random.Next(0, 11);

                if (firstRoll == 10)
                {
                    Roll(firstRoll);
                }
                else
                {
                    int secondRoll = random.Next(0, 11 - firstRoll);
                    Roll(firstRoll);
                    Roll(secondRoll);
                }
            }

            int lastRoundFirstRoll = random.Next(0, 11);
            Roll(lastRoundFirstRoll);

            if (lastRoundFirstRoll == 10)
            {
                int lastRoundSecondRoll = random.Next(0, 11);
                Roll(lastRoundSecondRoll);
                int lastRoundThirdRoll = random.Next(0, 11);
                Roll(lastRoundThirdRoll);
            }
            else
            {
                int lastRoundSecondRoll = random.Next(0, 11 - lastRoundFirstRoll);
                Roll(lastRoundSecondRoll);

                if (lastRoundFirstRoll + lastRoundSecondRoll == 10)
                {
                    int lastRoundThirdRoll = random.Next(0, 11);
                    Roll(lastRoundThirdRoll);
                }
            }
        }
    }
}