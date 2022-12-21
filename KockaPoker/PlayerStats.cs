using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KockaPoker
{
    internal class PlayerStats
    {
        /* Saját kockagenerálás mindkettő játékosnak */
        static Random diceGenerator = new Random();

        /* Minden játékosnak külön kockái vannak */
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
            /* X választott számok összeadása */
            int sum = 0;
            foreach (int current in dices)
                if (current == selectedNumber)
                    sum += current;

            return sum;
        }
        public int ScoreKinds(int[] dices, int countKind)
        {
            /* Ha van X db szám akkor összeadjuk őket */
            int[] countNumbers = new int[7];
            foreach (int current in dices)
                countNumbers[current]++;

            foreach (int current in countNumbers)
                if (current == countKind)
                    return dices.Sum();

            return 0;
        }
        public int ScoreFullHouse(int[] dices) => /* Előző funkciókat felhasználva */
            (ScoreKinds(dices, 2) != 0 && ScoreKinds(dices, 3) != 0) ? 25 : 0;
        public int ScoreStraight(int[] dices, int straightCount)
        {
            /* Kell egy ideiglenes tömb, vagy össze fogja kutyulni a dobásokat */
            int[] tempArray = { dices[0], dices[1], dices[2], dices[3], dices[4] };

            /* Ez a függvény miatt kell az ideiglenes lista */
            /* Az "Array.Sort(Dices)" az eredeti tömböt is megpiszkálja */
            Array.Sort(tempArray);

            /* Ha előző egyel nagyobb és azoknak megszámlálása hányszor fordul elő */
            int length = 1;
            for (int i = 1; i < tempArray.Length; i++)
                if (tempArray[i - 1] + 1 == tempArray[i])
                    length++;

            /* Nagy vagy kis sor */
            if (length == straightCount)
                return straightCount == 4 ? 30 : 40;

            return 0;
        }
        public int ScoreYahtzee(int[] dices) => /* 5db ugyanolyan előző funkcióval */
            ScoreKinds(dices, 5) != 0 ? 50 : 0;
        public int ScoreChance(int[] dices) => /* Összes kocka összeadva */
            dices.Sum();
        public void GenerateDiceParts(bool[] regenerate)
        {
            /* Újragenerálni a nem kiválasztott kockadarabokat */

            /* Ha még nincs kiválasztott (regenerate == null) */
            if (regenerate.Length == 0)
                for (int i = 0; i < CurrentDices.Length; i++)
                    CurrentDices[i] = diceGenerator.Next(1, 7);
            else
                for (int i = 0; i < CurrentDices.Length; i++)
                    if (!regenerate[i])
                        CurrentDices[i] = diceGenerator.Next(1, 7);
        }
    }
}
