using System.Threading.Channels;

namespace KockaPoker
{
    public partial class Yahtzee : Form
    {
        static PlayerStats player1 = new();
        public bool[] isActiveButton = new bool[5];
        static int chance = 4;

        public Yahtzee()
        {
            InitializeComponent();
        }

        private void throwButton_Click(object sender, EventArgs e)
        {
            if (chance > 0)
            {
                chance--;
                changeCounterLabel.Text = chance.ToString();

                player1.GenerateDiceParts(isActiveButton);

                generatedButton1.Text = player1.CurrentDices[0].ToString();
                generatedButton2.Text = player1.CurrentDices[1].ToString();
                generatedButton3.Text = player1.CurrentDices[2].ToString();
                generatedButton4.Text = player1.CurrentDices[3].ToString();
                generatedButton5.Text = player1.CurrentDices[4].ToString();

                CheckScores(player1.CurrentDices);
            }
            else
            {
                MessageBox.Show("Kifogytál a probálkozásokból!");
            }
        }
        private void generatedButton1_Click(object sender, EventArgs e)
        {
            
            if(generatedButton1.Text != "")
            {
                isActiveButton[0] = !isActiveButton[0];
                possChanging((Button)sender, 0);
            }
        }
        private void generatedButton2_Click(object sender, EventArgs e)
        {
            if (generatedButton1.Text != "")
            {
                isActiveButton[1] = !isActiveButton[1];
                possChanging((Button)sender, 1);
            }

        }

        private void generatedButton3_Click(object sender, EventArgs e)
        {
            if (generatedButton1.Text != "")
            {
                isActiveButton[2] = !isActiveButton[2];
                possChanging((Button)sender, 2);
            }
        }

        private void generatedButton4_Click(object sender, EventArgs e)
        {
            if (generatedButton1.Text != "")
            {
                isActiveButton[3] = !isActiveButton[3];
                possChanging((Button)sender, 3);
            }
        }

        private void generatedButton5_Click(object sender, EventArgs e)
        {
            if (generatedButton1.Text != "")
            {
                isActiveButton[4] = !isActiveButton[4];
                possChanging((Button)sender, 4);
            }
        }
        public void possChanging(Button sender, int index) =>
            sender.Location = new Point(sender.Location.X, isActiveButton[index] ? sender.Location.Y + 10 : sender.Location.Y - 10);

        private void Yahtzee_Load(object sender, EventArgs e)
        {

        }

        private void CheckScores(int[] dices)
        {
            player1Ones.Text = player1.ScoreNumbers(dices, 1).ToString();
            player1Twos.Text = player1.ScoreNumbers(dices, 2).ToString();
            player1Threes.Text = player1.ScoreNumbers(dices, 3).ToString();
            player1Fours.Text = player1.ScoreNumbers(dices, 4).ToString();
            player1Fives.Text = player1.ScoreNumbers(dices, 5).ToString();
            player1Sixes.Text = player1.ScoreNumbers(dices, 6).ToString();

            player1ThreeKind.Text = player1.ScoreKinds(dices, 3).ToString();
            player1FourKind.Text = player1.ScoreKinds(dices, 4).ToString();
            player1FullHouse.Text = player1.ScoreFullHouse(dices).ToString();
            player1SmallStraight.Text = player1.ScoreStraight(dices, 4).ToString();
            player1LargeStraight.Text = player1.ScoreStraight(dices, 5).ToString();
            player1Yahtzee.Text = player1.ScoreYahtzee(dices).ToString();
            player1Chance.Text = player1.ScoreChance(dices).ToString();
        }

        private void SaveLock(object sender, EventArgs e)
        {
            Button current = (Button)sender;

            generatedButton5.Text = generatedButton4.Text = generatedButton3.Text = generatedButton2.Text = generatedButton1.Text = "";

            chance = 4;
        }
    }
}