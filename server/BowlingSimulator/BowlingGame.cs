namespace BowlingSimulator
{
    public class BowlingGame
    {
        private List<int> rolls = new List<int>();

        public void Roll(int pins)
        {
            rolls.Add(pins);
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
    }
}