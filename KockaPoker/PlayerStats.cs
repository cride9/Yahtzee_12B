using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KockaPoker
{
    internal class PlayerStats
    {
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

        public int ScoreThreeKind(int[] dices)
        {
            int[] countNumbers = new int[6];
            foreach (int current in dices)
                countNumbers[current]++;

            foreach (int current in countNumbers)
                if (current >= 3)
                    return dices.Sum();

            return 0;
        }
    }
}
