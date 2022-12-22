using System;
using System.Threading.Channels;

namespace KockaPoker
{
    public partial class Yahtzee : Form
    {
        /* Gomb inform�ci�k t�rol�s�ra */
        static Dictionary<Button, bool> buttonInformations = new();
        static Dictionary<Button, bool> lockedInformations = new();

        /* J�t�kos l�trehoz�sa */
        static PlayerStats player1 = new();

        /* J�t�kos hozz�ad�sa */
        static bool isMultiplayerChechked = false;

        /* Pr�b�lkoz�sok (1dob�s 3 �jradob�s) */
        static int chance = 4;
        static bool alreadyLocked = false;
        static bool reset = false;

        public Yahtzee()
        {
            InitializeComponent();
        }

        /* Dob�s gomb */
        private void throwButton_Click(object sender, EventArgs e)
        {
            /* V�ge a j�t�knak check */
            if (!lockedInformations.ContainsValue(true))
            {
                MessageBox.Show("V�ge a j�t�knak");
                return;
            }

            /* Van-e m�g pr�b�lkoz�s */
            if (chance == 0)
            {
                MessageBox.Show("Kifogyt�l a prob�lkoz�sokb�l!");
                return;
            }

            /* �jra lehessen v�lasztani */
            alreadyLocked = false;

            /* Levonjuk a pr�b�lkoz�st */
            chance--;

            /* Vizualiz�l�s */
            p1ChangeCounterLabel.Text = chance.ToString();

            /* Gener�ljuk �jra azokat, amiket a j�t�kos akar */
            player1.GenerateDiceParts(buttonInformations.Values.ToArray());

            /* Megv�ltoztatni a dobott �sszegekre */
            p1GeneratedButton1.Text = player1.CurrentDices[0].ToString();
            p1GeneratedButton2.Text = player1.CurrentDices[1].ToString();
            p1GeneratedButton3.Text = player1.CurrentDices[2].ToString();
            p1GeneratedButton4.Text = player1.CurrentDices[3].ToString();
            p1GeneratedButton5.Text = player1.CurrentDices[4].ToString();

            /* Megn�zni miket lehet vel�k csin�lni */
            CheckScores(player1.CurrentDices);
        }
        
        /* Kiv�laszt�s vizualiz�l�sa (lejjebb megy a gomb) */
        public void possChanging(object sender, EventArgs e)
        {
            /* Jelenlegi gomb, amivel interakci�ba l�pt�nk */
            Button current = (Button)sender;

            /* Ha m�g nem dobtunk, ne lehessen kiv�lasztani */
            if (current.Text == "" && !reset)
                return;

            /* Kiv�laszt�s kezel�s */
            buttonInformations[current] = !buttonInformations[current];

            /* Poz�ci� v�ltoztat�s */
            current.Location = new Point(current.Location.X, buttonInformations[current] ? current.Location.Y + 10 : current.Location.Y - 10);
        }

        /* Eredm�nyek vizsg�lata */
        private void CheckScores(int[] dices)
        {
            /* Mindegyik gombnak be�rjuk a pontok �sszeg�t amit tudunk vel�k szerezni */
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

        /* Kiv�laszt�s */
        private void SaveLock(object sender, EventArgs e)
        {
            /* Ha m�r lett v�lasztva ne engedje */
            if (alreadyLocked)
            {
                ResetGeneratedNumbers();
                return;
            }

            /* Jelenlegi gomb, amivel interakci�ba l�pt�nk */
            Button current = (Button)sender;

            /* Ez a gomb t�bb� nem megnyomhat� */
            current.Enabled = false;

            /* Vizualiz�l�s */
            current.BackColor = Color.White;

            /* A dobott �sszegek null�z�sa */
            p1GeneratedButton5.Text = p1GeneratedButton4.Text = p1GeneratedButton3.Text = p1GeneratedButton2.Text = p1GeneratedButton1.Text = "";
           
            /* Visszakapja a 4dob�st */
            chance = 4;
            p1ChangeCounterLabel.Text = (chance - 1).ToString();

            /* Ez a gomb m�r haszn�lva lett */
            lockedInformations[current] = false;

            /* Ne haszn�lhassunk t�bb gombot */
            alreadyLocked = true;
            ResetGeneratedNumbers();
        }

        /* K�nyvt�rak felt�lt�se minden gomb adattal */
        private void Yahtzee_Load(object sender, EventArgs e)
        {
            buttonInformations.Add(p1GeneratedButton1, false);
            buttonInformations.Add(p1GeneratedButton2, false);
            buttonInformations.Add(p1GeneratedButton3, false);
            buttonInformations.Add(p1GeneratedButton4, false);
            buttonInformations.Add(p1GeneratedButton5, false);

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

        void ResetGeneratedNumbers()
        {
            /* Nem j�tkos kattint�s */
            reset = true;

            /* A kiv�lasztott kock�kat visszarakni a sima �llapotukba (feltolni) */
            foreach (var item in buttonInformations)
                if (buttonInformations[item.Key])
                    item.Key.PerformClick();

            reset = false;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (isMultiplayerChechked)
            {
                p2Panel.Visible = false;
                isMultiplayerChechked = false;

                return;
            }

            p2Panel.Visible = true;
            isMultiplayerChechked = true;
        }
    }
}