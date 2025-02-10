
namespace BowlingSimulator
{
    public class BowlingGame
    {
        private List<int> rolls = new List<int>();
        private Random random = new Random();

        public void Roll(int pins) => rolls.Add(pins);

        public void Reset() => rolls.Clear();

        public int RollBall()
        {
            int maxPins = 10;
            int ballCount = rolls.Count;
            int round = 0;
            int i = 0;

          
            while (i < ballCount && round < 9)
            {
                if (rolls[i] == 10) 
                {
                    i++;
                    round++;
                }
                else
                {
                    if (i + 1 < ballCount)
                    {
                        i += 2;
                        round++;
                    }
                    else
                    {
                        
                        break;
                    }
                }
            }

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
                
                        maxPins = 10;
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

            int pins = random.Next(0, maxPins + 1);
            Roll(pins);
            return pins;
        }

        public List<int> GetRolls() => new List<int>(rolls);

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

        private int GetRoll(int index)
        {
            return index < rolls.Count ? rolls[index] : 0;
        }
    }
}
