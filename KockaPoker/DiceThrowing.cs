using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KockaPoker
{
    public class DiceThrowing
    {
        /* Statikusnak kell lennie vagy mindig ugyanazokat generálná */
        static Random diceGenerator = new Random();

        public int[] CurrentDices = new int[5];

        public DiceThrowing()
        {
            for (int i = 0; i < CurrentDices.Length; i++)
                CurrentDices[i] = diceGenerator.Next(1, 7);
        }

        public void GenerateDiceParts(bool[] regenerate)
        {
            for (int i = 0; i < CurrentDices.Length; i++)
                if (regenerate[i])
                    CurrentDices[i] = diceGenerator.Next(1, 7);
        }
    }
}
