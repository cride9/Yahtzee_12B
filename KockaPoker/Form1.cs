using System;
using System.Threading.Channels;

namespace KockaPoker
{
    public partial class Yahtzee : Form
    {
        static Dictionary<Button, bool> buttonInformations = new();
        static Dictionary<Button, bool> lockedInformations = new();
        static PlayerStats player1 = new();
        static int chance = 4;

        public Yahtzee()
        {
            InitializeComponent();
        }

        private void throwButton_Click(object sender, EventArgs e)
        {
            if (chance == 0)
            {
                MessageBox.Show("Kifogytál a probálkozásokból!");
                return;
            }

            chance--;
            changeCounterLabel.Text = chance.ToString();

            player1.GenerateDiceParts(buttonInformations.Values.ToArray());

            generatedButton1.Text = player1.CurrentDices[0].ToString();
            generatedButton2.Text = player1.CurrentDices[1].ToString();
            generatedButton3.Text = player1.CurrentDices[2].ToString();
            generatedButton4.Text = player1.CurrentDices[3].ToString();
            generatedButton5.Text = player1.CurrentDices[4].ToString();

            CheckScores(player1.CurrentDices);
        }
        
        public void possChanging(object sender, EventArgs e)
        {
            Button current = (Button)sender;
            buttonInformations[current] = !buttonInformations[current];
            current.Location = new Point(current.Location.X, buttonInformations[current] ? current.Location.Y + 10 : current.Location.Y - 10);
        }

        private void CheckScores(int[] dices)
        {
            player1Ones.Text = lockedInformations[player1Ones] ? player1.ScoreNumbers(dices, 1).ToString() : player1Ones.Text;
            player1Twos.Text = lockedInformations[player1Twos] ? player1.ScoreNumbers(dices, 2).ToString() : player1Twos.Text;
            player1Threes.Text = lockedInformations[player1Threes] ? player1.ScoreNumbers(dices, 3).ToString() : player1Threes.Text;
            player1Fours.Text = lockedInformations[player1Fours] ? player1.ScoreNumbers(dices, 4).ToString() : player1Fours.Text;
            player1Fives.Text = lockedInformations[player1Fives] ? player1.ScoreNumbers(dices, 5).ToString() : player1Fives.Text;
            player1Sixes.Text = lockedInformations[player1Sixes] ? player1.ScoreNumbers(dices, 6).ToString() : player1Sixes.Text;

            player1ThreeKind.Text = lockedInformations[player1ThreeKind] ? player1.ScoreKinds(dices, 3).ToString() : player1ThreeKind.Text;
            player1FourKind.Text = lockedInformations[player1FourKind] ? player1.ScoreKinds(dices, 4).ToString() : player1FourKind.Text;
            player1FullHouse.Text = lockedInformations[player1FullHouse] ? player1.ScoreFullHouse(dices).ToString() : player1FullHouse.Text;
            player1SmallStraight.Text = lockedInformations[player1SmallStraight] ? player1.ScoreStraight(dices, 4).ToString() : player1SmallStraight.Text;
            player1LargeStraight.Text = lockedInformations[player1LargeStraight] ? player1.ScoreStraight(dices, 5).ToString() : player1LargeStraight.Text;
            player1Yahtzee.Text = lockedInformations[player1Yahtzee] ? player1.ScoreYahtzee(dices).ToString() : player1Yahtzee.Text;
            player1Chance.Text = lockedInformations[player1Chance] ? player1.ScoreChance(dices).ToString() : player1Chance.Text;
        }

        private void SaveLock(object sender, EventArgs e)
        {
            Button current = (Button)sender;

            current.Enabled = false;
            current.BackColor = Color.White;

            generatedButton5.Text = generatedButton4.Text = generatedButton3.Text = generatedButton2.Text = generatedButton1.Text = "";
           
            chance = 4;
            
            changeCounterLabel.Text = chance.ToString();

            foreach (var item in buttonInformations)
                if (buttonInformations[item.Key])
                    item.Key.PerformClick();

            lockedInformations[current] = false;
        }

        private void Yahtzee_Load(object sender, EventArgs e)
        {
            buttonInformations.Add(generatedButton1, false);
            buttonInformations.Add(generatedButton2, false);
            buttonInformations.Add(generatedButton3, false);
            buttonInformations.Add(generatedButton4, false);
            buttonInformations.Add(generatedButton5, false);

            lockedInformations.Add(player1Ones, true);
            lockedInformations.Add(player1Twos, true);
            lockedInformations.Add(player1Threes, true);
            lockedInformations.Add(player1Fours, true);
            lockedInformations.Add(player1Fives, true);
            lockedInformations.Add(player1Sixes, true);
            lockedInformations.Add(player1ThreeKind, true);
            lockedInformations.Add(player1FourKind, true);
            lockedInformations.Add(player1FullHouse, true);
            lockedInformations.Add(player1SmallStraight, true);
            lockedInformations.Add(player1LargeStraight, true);
            lockedInformations.Add(player1Yahtzee, true);
            lockedInformations.Add(player1Chance, true);
        }
    }
}