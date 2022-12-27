using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace KockaPoker {

    enum ThinkFlags : int {

        NONE = 0, 
        THROW_NEW = (1 << 1),

        KEEP_FOR_ONES = (1 << 2),
        KEEP_FOR_TWOS = (1 << 3),
        KEEP_FOR_THREES = (1 << 4),
        KEEP_FOR_FOURS = (1 << 5),
        KEEP_FOR_FIVES = (1 << 6),
        KEEP_FOR_SIXES = (1 << 7),

        KEEP_FOR_THREEKIND = (1 << 8),
        KEEP_FOR_FOURKIND = (1 << 9),

        KEEP_FOR_FULLHOUSE = (1 << 10),
        KEEP_FOR_ANY_STRAIGHT = (1 << 11),
        KEEP_FOR_SMALL_STRAIGHT = (1 << 12),
        KEEP_FOR_LARGE_STRAIGHT = (1 << 13),
        KEEP_FOR_YAHTZEE = (1 << 14),
        KEEP_FOR_CHANCE = (1 << 15)
    }
    enum ScoreFlags : int {

        NONE = 0,
        SCORE_ONES = (1 << 1),
        SCORE_TWOS = (1 << 2),
        SCORE_THREES = (1 << 3),
        SCORE_FOURS = (1 << 4),
        SCORE_FIVES = (1 << 5),
        SCORE_SIXES = (1 << 6),

        SCORE_THREEKIND = (1 << 7),
        SCORE_FOURKIND = (1 << 8),

        SCORE_FULLHOUSE = (1 << 9),
        SCORE_SMALL_STRAIGHT = (1 << 10),
        SCORE_LARGE_STRAIGHT = (1 << 11),
        SCORE_YAHTZEE = (1 << 12),
        SCORE_CHANCE = (1 << 13)
    }
 
    public class ArtificalOpponent {

        Yahtzee Game;
        GameInformations info;
        playerID ID;

        ThinkFlags thinkFlags;
        ScoreFlags scoreFlags;

        int[] numberCounts = new int[5];

        /* Bot player */
        public ArtificalOpponent(GameInformations gameInformation, playerID playerID, Yahtzee game) {

            info = gameInformation;
            ID = playerID;
            Game = game;

            RunAlgorithm();
        }
        private void RunAlgorithm() {

            PreThink();
            Think();
            PostThink();
        }
        private void PreThink() {

            var dices = info.GetPlayer(ID).CurrentDices;
            var scores = info.GetLockInformations(ID);

            /* Melyik számból mennyi van? */
            for (int i = 0; i < dices.Length; i++)
                numberCounts[i] = FindSameNumbers(dices, i + 1);

            /*
                index = melyik szám
                érték = hány darab szám
            */

            /* Yatzee */
            if (scores[Game.p2Yahtzee]) {
                if (numberCounts.Contains(5))
                    scoreFlags |= ScoreFlags.SCORE_YAHTZEE;
            }
            else {
                for (int i = 0; i < numberCounts.Length; i++) {

                    if (numberCounts[i] == 5) {
                        switch (i) {
                            case 0:
                                if (scores[Game.p2Ones])
                                    scoreFlags |= ScoreFlags.SCORE_ONES;
                                break;
                            case 1:
                                if (scores[Game.p2Twos])
                                    scoreFlags |= ScoreFlags.SCORE_TWOS;
                                break;
                            case 2:
                                if (scores[Game.p2Threes])
                                    scoreFlags |= ScoreFlags.SCORE_THREES;
                                break;
                            case 3:
                                if (scores[Game.p2Fours])
                                    scoreFlags |= ScoreFlags.SCORE_FOURS;
                                break;
                            case 4:
                                if (scores[Game.p2Fives])
                                    scoreFlags |= ScoreFlags.SCORE_FIVES;
                                break;
                            case 5:
                                if (scores[Game.p2Sixes])
                                    scoreFlags |= ScoreFlags.SCORE_SIXES;
                                break;
                        }
                    }
                }
            }

            /* FourKind */
            if (scores[Game.p2FourKind]) {
                if (numberCounts.Contains(4))
                    scoreFlags |= ScoreFlags.SCORE_FOURKIND;
            }

            /* ThreeKind */
            if (scores[Game.p2ThreeKind]) {
                if (numberCounts.Contains(3))
                    scoreFlags |= ScoreFlags.SCORE_THREEKIND;
            }


            /* Full House */
            if (scores[Game.p2FullHouse]) {
                if (numberCounts.Contains(3) && numberCounts.Contains(2))
                    scoreFlags |= ScoreFlags.SCORE_FULLHOUSE;
            }


            /* Chance if throw > 20 */
            if (scores[Game.p2Chance]) {
                if (dices.Sum() > 20)
                    scoreFlags |= ScoreFlags.SCORE_CHANCE;
            }

            /* Small Straight */
            if (scores[Game.p2SmallStraight]) {
                if (int.Parse(Game.p2SmallStraight.Text) != 0)
                    scoreFlags |= ScoreFlags.SCORE_SMALL_STRAIGHT;
            }

            /* Large Straight */
            if (scores[Game.p2LargeStraight]) {
                if (int.Parse(Game.p2LargeStraight.Text) != 0)
                    scoreFlags |= ScoreFlags.SCORE_LARGE_STRAIGHT;
            }
        }
        private void Think() {

            /* Ha választhat valamit akkor ez ne is fusson le */
            if (scoreFlags != ScoreFlags.NONE)
                return;
        }
        private void PostThink() {

        }

        private int FindSameNumbers(int[] dices, int number) {

            int same = 0;
            foreach (var item in dices)
                if (item == number)
                    same++;

            return same;
        }
    }
}
