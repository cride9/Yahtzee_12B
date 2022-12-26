using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace KockaPoker
{
    enum ThinkFlags : byte
    {
        NONE,
        THROW_NEW,

        KEEP_FOR_ONES,
        KEEP_FOR_TWOS,
        KEEP_FOR_THREES,
        KEEP_FOR_FOURS,
        KEEP_FOR_FIVES,
        KEEP_FOR_SIXES,

        KEEP_FOR_THREEKIND,
        KEEP_FOR_FOURKIND,

        KEEP_FOR_FULLHOUSE,
        KEEP_FOR_ANY_STRAIGHT,
        KEEP_FOR_SMALL_STRAIGHT,
        KEEP_FOR_LARGE_STRAIGHT,
        KEEP_FOR_YAHTZEE,
        KEEP_FOR_CHANCE
    }
    enum ScoreFlags : byte
    {
        NONE,
        SCORE_ONES,
        SCORE_TWOS,
        SCORE_THREES,
        SCORE_FOURS,
        SCORE_FIVES,
        SCORE_SIXES,

        SCORE_THREEKIND,
        SCORE_FOURKIND,

        SCORE_FULLHOUSE,
        SCORE_SMALL_STRAIGHT,
        SCORE_LARGE_STRAIGHT,
        SCORE_YAHTZEE,
        SCORE_CHANCE
    }
 
    public class ArtificalOpponent : Yahtzee
    {
        static GameInformations info = new();
        static Dictionary<Button, bool> buttonInformations = new Dictionary<Button, bool>();
        playerID ID = 0;
        ThinkFlags think = ThinkFlags.NONE;
        ScoreFlags score = ScoreFlags.NONE;

        /* Bot player */
        public ArtificalOpponent(ref GameInformations gameInformation, playerID playerID )
        {
            info = gameInformation;
            buttonInformations = gameInformation.GetLockInformations(playerID);
            ID = playerID;
        }

        public void RunAlgorithm()
        {
            Thread.Sleep(500);
            PreThink();
            Thread.Sleep(500);
            Think();
            Thread.Sleep(500);
            PostThink();
        }
        private void PreThink()
        {
            int mostNumberCount = 0;
            int mostNumber = 0;
            bool twoSame = false;

            PlayerStats playerInfo = info.GetPlayer(ID);
            int[] currentDices = playerInfo.CurrentDices;

            for (int i = 1; i < 6; i++)
            {
                int sameNumberCount = FindSameNumbers(info.GetPlayer(ID).CurrentDices, i);

                if (mostNumberCount < sameNumberCount)
                {
                    mostNumberCount = sameNumberCount;
                    mostNumber = currentDices[i - 1];
                }

                if (sameNumberCount == 2)
                    twoSame = true;
            }

            if (mostNumberCount == 1)
            {
                if (buttonInformations[p2SmallStraight] && playerInfo.ScoreStraight(currentDices, 4) != 0)
                    score = ScoreFlags.SCORE_SMALL_STRAIGHT;
                else if (buttonInformations[p2LargeStraight] && playerInfo.ScoreStraight(currentDices, 5) != 0)
                    score = ScoreFlags.SCORE_LARGE_STRAIGHT;
                else if (buttonInformations[p2Chance] && currentDices.Sum() >= 20)
                    score = ScoreFlags.SCORE_CHANCE;
            }
            else if (mostNumberCount == 2)
            {
                if (buttonInformations[p2Chance] && playerInfo.ScoreChance(currentDices) != 0)
                    score = ScoreFlags.SCORE_CHANCE;
            }
            else if (mostNumberCount == 3)
            {
                if (buttonInformations[p2FullHouse] && twoSame)
                    score = ScoreFlags.SCORE_FULLHOUSE;
                else if (buttonInformations[p2ThreeKind] && playerInfo.ScoreNumbers(currentDices, mostNumber) != 0)
                    score = ScoreFlags.SCORE_THREEKIND;
            }
            else if (mostNumber == 4)
            {
                if (buttonInformations[p2FourKind] && playerInfo.ScoreNumbers(currentDices, mostNumber) != 0)
                    score = ScoreFlags.SCORE_FOURKIND;
                else
                {
                    switch (mostNumber)
                    {
                        case 1:
                            if (buttonInformations[p2Ones])
                                score = ScoreFlags.SCORE_ONES;
                            break;
                        case 2:
                            if (buttonInformations[p2Twos])
                                score = ScoreFlags.SCORE_TWOS;
                            break;
                        case 3:
                            if (buttonInformations[p2Threes])
                                score = ScoreFlags.SCORE_THREES;
                            break;
                        case 4:
                            if (buttonInformations[p2Fours])
                                score = ScoreFlags.SCORE_FOURS;
                            break;
                        case 5:
                            if (buttonInformations[p2Fives])
                                score = ScoreFlags.SCORE_FIVES;
                            break;
                        case 6:
                            if (buttonInformations[p2Sixes])
                                score = ScoreFlags.SCORE_SIXES;
                            break;
                    }
                }

            }
            else if (mostNumber == 5)
            {
                if (buttonInformations[p2Yahtzee] && playerInfo.ScoreNumbers(currentDices, mostNumber) != 0)
                    score = ScoreFlags.SCORE_YAHTZEE;
                else
                {
                    switch (mostNumber)
                    {
                        case 1:
                            if (buttonInformations[p2Ones])
                                score = ScoreFlags.SCORE_ONES;
                            break;
                        case 2:
                            if (buttonInformations[p2Twos])
                                score = ScoreFlags.SCORE_TWOS;
                            break;
                        case 3:
                            if (buttonInformations[p2Threes])
                                score = ScoreFlags.SCORE_THREES;
                            break;
                        case 4:
                            if (buttonInformations[p2Fours])
                                score = ScoreFlags.SCORE_FOURS;
                            break;
                        case 5:
                            if (buttonInformations[p2Fives])
                                score = ScoreFlags.SCORE_FIVES;
                            break;
                        case 6:
                            if (buttonInformations[p2Sixes])
                                score = ScoreFlags.SCORE_SIXES;
                            break;
                    }
                }
            }
        }
        private void Think()
        {
            MessageBox.Show(((int)score).ToString());
        }
        private void PostThink()
        {

        }

        private int FindSameNumbers(int[] dices, int number)
        {
            int same = 0;
            foreach (var item in dices)
            {
                if (item == number)
                    same++;
            }
            return same;
        }
    }
}
