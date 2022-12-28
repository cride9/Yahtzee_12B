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
 
    public class ArtificalOpponent : Yahtzee {

        Yahtzee Game;
        GameInformations info;
        playerID ID;

        ThinkFlags thinkFlags;
        ScoreFlags scoreFlags;

        int[] numberCounts = new int[5];
        bool panicScore = false;

        /* Bot player */
        public ArtificalOpponent(GameInformations gameInformation, playerID playerID, Yahtzee game) {

            info = gameInformation;
            ID = playerID;
            Game = game;

            RunAlgorithm();
        }
        private void RunAlgorithm() {

            Game.player2ThrowButton.PerformClick();
            PreThink();
            Think();
            PostThink();
        }
        private void PreThink() {

            panicScore = false;
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

            /* Chance if throw > 20 */
            if (scores[Game.p2Chance]) {
                if (dices.Sum() > 20)
                    scoreFlags |= ScoreFlags.SCORE_CHANCE;
            }
        }
        private void Think() {

            /* Ha választhat valamit akkor ez ne is fusson le */
            if (scoreFlags != ScoreFlags.NONE)
                return;

            var dices = info.GetPlayer(ID).CurrentDices;
            var scores = info.GetLockInformations(ID);

            /* Ha van 2db akkor mik lehetnek? */
            if (numberCounts.Contains(2)) {

                if (scores[Game.p2FullHouse]) {

                    thinkFlags |= ThinkFlags.KEEP_FOR_FULLHOUSE;
                }
                if (scores[Game.p2ThreeKind]) {

                    thinkFlags |= ThinkFlags.KEEP_FOR_THREEKIND;
                }
                if (scores[Game.p2FourKind]) {

                    thinkFlags |= ThinkFlags.KEEP_FOR_FOURKIND;
                }
            }

            /* Ha van sok ugyanolyan */
            if (info.GetPlayer(ID).ScoreStraight(dices, 3) != 0) {

                if (scores[Game.p2SmallStraight])
                    thinkFlags |= ThinkFlags.KEEP_FOR_SMALL_STRAIGHT;
            }

            /* Ha van sok ugyanolyan, de nincs már kis sor */
            if (info.GetPlayer(ID).ScoreStraight(dices, 4) != 0 && !scores[Game.p2SmallStraight] && scores[Game.p2LargeStraight]) {

                thinkFlags |= ThinkFlags.KEEP_FOR_LARGE_STRAIGHT;
            }
        }
        private void PostThink() {

            /* Lebontani a biteket, miket tudunk csinálni és miket nem */
            if (scoreFlags > (scoreFlags &= ~ScoreFlags.SCORE_ONES)) {

                Game.p2Ones.PerformClick();
                return;
            }
            if (scoreFlags > (scoreFlags &= ~ScoreFlags.SCORE_TWOS)) {

                Game.p2Twos.PerformClick();
                return;
            }
            if (scoreFlags > (scoreFlags &= ~ScoreFlags.SCORE_THREES)) {

                Game.p2Threes.PerformClick();
                return;
            }
            if (scoreFlags > (scoreFlags &= ~ScoreFlags.SCORE_FOURS)) {

                Game.p2Fours.PerformClick();
                return;
            }
            if (scoreFlags > (scoreFlags &= ~ScoreFlags.SCORE_FIVES)) {

                Game.p2Fives.PerformClick();
                return;
            }
            if (scoreFlags > (scoreFlags &= ~ScoreFlags.SCORE_SIXES)) {

                Game.p2Sixes.PerformClick();
                return;
            }
            if (scoreFlags > (scoreFlags &= ~ScoreFlags.SCORE_THREEKIND)) {

                Game.p2ThreeKind.PerformClick();
                return;
            }
            if (scoreFlags > (scoreFlags &= ~ScoreFlags.SCORE_FOURKIND)) {

                Game.p2FourKind.PerformClick();
                return;
            }
            if (scoreFlags > (scoreFlags &= ~ScoreFlags.SCORE_FULLHOUSE)) {

                Game.p2FullHouse.PerformClick();
                return;
            }
            if (scoreFlags > (scoreFlags &= ~ScoreFlags.SCORE_SMALL_STRAIGHT)) {

                Game.p2SmallStraight.PerformClick();
                return;
            }
            if (scoreFlags > (scoreFlags &= ~ScoreFlags.SCORE_LARGE_STRAIGHT)) {

                Game.p2LargeStraight.PerformClick();
                return;
            }
            if (scoreFlags > (scoreFlags &= ~ScoreFlags.SCORE_YAHTZEE)) {

                Game.p2Yahtzee.PerformClick();
                return;
            }
            if (scoreFlags > (scoreFlags &= ~ScoreFlags.SCORE_CHANCE)) {

                Game.p2Chance.PerformClick();
                return;
            }

            if (info.GetPlayer(ID).ThrowChance != 0) {

                thinkFlags = 0;
                scoreFlags = 0;

                RunAlgorithm();
            }

            if (info.GetPlayer(ID).ThrowChance == 0 && !panicScore) {

                panicScore = true;
                ScoreLargest();
                PostThink();
            }
        }

        private void ScoreLargest() {

            var scores = info.GetLockInformations(ID);

            int largest = 0;

            if (scores[Game.p2Ones]) {
                if (int.Parse(Game.p2Ones.Text) >= largest) {
                    scoreFlags = 0;
                    largest = int.Parse(Game.p2Ones.Text);
                    scoreFlags |= ScoreFlags.SCORE_ONES;
                }
            }


            if (scores[Game.p2Twos]) {
                if (int.Parse(Game.p2Twos.Text) >= largest) {
                    scoreFlags = 0;
                    largest = int.Parse(Game.p2Twos.Text);
                    scoreFlags |= ScoreFlags.SCORE_TWOS;
                }
            }

            if (scores[Game.p2Threes]) {
                if (int.Parse(Game.p2Threes.Text) >= largest) {
                    scoreFlags = 0;
                    largest = int.Parse(Game.p2Threes.Text);
                    scoreFlags |= ScoreFlags.SCORE_THREES;
                }
            }


            if (scores[Game.p2Fours]) {
                if (int.Parse(Game.p2Fours.Text) >= largest) {
                    scoreFlags = 0;
                    largest = int.Parse(Game.p2Fours.Text);
                    scoreFlags |= ScoreFlags.SCORE_FOURS;
                }
            }


            if (scores[Game.p2Fives]) {
                if (int.Parse(Game.p2Fives.Text) >= largest) {
                    scoreFlags = 0;
                    largest = int.Parse(Game.p2Fives.Text);
                    scoreFlags |= ScoreFlags.SCORE_FIVES;
                }
            }


            if (scores[Game.p2Sixes]) {
                if (int.Parse(Game.p2Sixes.Text) >= largest) {
                    scoreFlags = 0;
                    largest = int.Parse(Game.p2Sixes.Text);
                    scoreFlags |= ScoreFlags.SCORE_SIXES;
                }
            }


            if (scores[Game.p2ThreeKind]) {
                if (int.Parse(Game.p2ThreeKind.Text) >= largest) {
                    scoreFlags = 0;
                    largest = int.Parse(Game.p2ThreeKind.Text);
                    scoreFlags |= ScoreFlags.SCORE_THREEKIND;
                }
            }


            if (scores[Game.p2FourKind]) {
                if (int.Parse(Game.p2FourKind.Text) >= largest) {
                    scoreFlags = 0;
                    largest = int.Parse(Game.p2FourKind.Text);
                    scoreFlags |= ScoreFlags.SCORE_FOURKIND;
                }
            }

            if (scores[Game.p2FullHouse]) {
                if (int.Parse(Game.p2FullHouse.Text) >= largest) {
                    scoreFlags = 0;
                    largest = int.Parse(Game.p2FullHouse.Text);
                    scoreFlags |= ScoreFlags.SCORE_FULLHOUSE;
                }
            }

            if (scores[Game.p2SmallStraight]) {
                if (int.Parse(Game.p2SmallStraight.Text) >= largest) {
                    scoreFlags = 0;
                    largest = int.Parse(Game.p2SmallStraight.Text);
                    scoreFlags |= ScoreFlags.SCORE_SMALL_STRAIGHT;
                }
            }

            if (scores[Game.p2LargeStraight]) {
                if (int.Parse(Game.p2LargeStraight.Text) >= largest) {
                    scoreFlags = 0;
                    largest = int.Parse(Game.p2LargeStraight.Text);
                    scoreFlags |= ScoreFlags.SCORE_LARGE_STRAIGHT;
                }
            }

            if (scores[Game.p2Yahtzee]) {
                if (int.Parse(Game.p2Yahtzee.Text) >= largest) {
                    scoreFlags = 0;
                    largest = int.Parse(Game.p2Yahtzee.Text);
                    scoreFlags |= ScoreFlags.SCORE_YAHTZEE;
                }
            }

            if (scores[Game.p2Chance]) {
                if (int.Parse(Game.p2Chance.Text) >= largest) {
                    scoreFlags = 0;
                    largest = int.Parse(Game.p2Chance.Text);
                    scoreFlags |= ScoreFlags.SCORE_CHANCE;
                }
            }
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
