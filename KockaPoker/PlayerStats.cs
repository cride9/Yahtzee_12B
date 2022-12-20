using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KockaPoker
{
    internal class PlayerStats
    {
        static Random diceGenerator = new Random();

        public int[] CurrentDices = new int[5];

        /* Felső rész */
        public int[] Numbers { get; } = new int[6];

        /* Alsó rész */
        public int ThreeKind { get; set; } /* ha van min 3db ugyanolyan akkor összeadja összes kockát */
        public int FourKind { get; set; } /* ha van min 4db ugyanolyan akkor összeadja összes kockát */
        public bool FullHouse { get; set; } /* fix 25 pont */
        public bool SmallStraight { get; set; } /* fix 30 pont */
        public bool LargeStraight { get; set; } /* fix 40 pont */
        public bool Yahtzee { get; set; } /* fix 50 pont */
        public int Chance { get; set; } /* összeadja az összeset */
        public int ScoreNumbers(int[] dices, int selectedNumber)
        {
            int sum = 0;
            foreach (int current in dices)
                if (current == selectedNumber)
                    sum += current;

            return sum;
        }
        public int ScoreKinds(int[] dices, int countKind)
        {
            int[] countNumbers = new int[7];
            foreach (int current in dices)
                countNumbers[current]++;

            foreach (int current in countNumbers)
                if (current == countKind)
                    return dices.Sum();

            return 0;
        }
        public int ScoreFullHouse(int[] dices) =>
            (ScoreKinds(dices, 2) != 0 && ScoreKinds(dices, 3) != 0) ? 25 : 0;
        public int ScoreStraight(int[] dices, int straightCount)
        {
            int orderNumbers = 1;
            for (int i = 0; i < dices.Length; i++)
                for(int j = 0; j < dices.Length; j++)
                    if (dices[i] + 1 == dices[j])
                        orderNumbers++;

            if (orderNumbers == straightCount)
                return straightCount == 4 ? 30 : 40;

            return 0;
        }
        public int ScoreYahtzee(int[] dices) =>
            ScoreKinds(dices, 5) != 0 ? 50 : 0;
        public int ScoreChance(int[] dices) =>
            dices.Sum();
        public void GenerateDiceParts(bool[] regenerate)
        {
            for (int i = 0; i < CurrentDices.Length; i++)
                if (!regenerate[i])
                    CurrentDices[i] = diceGenerator.Next(1, 7);
        }
    }
}
