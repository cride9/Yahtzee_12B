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
            sender.Location = new Point(sender.Location.X, isActiveButton[index] ? 440 : 430);

        private void Yahtzee_Load(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}