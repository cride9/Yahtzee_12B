using System;
using System.Threading.Channels;

namespace KockaPoker
{
    public partial class Yahtzee : Form
    {
        /* Gomb információk tárolására */
        static Dictionary<Button, bool> buttonInformations = new();
        static Dictionary<Button, bool> lockedInformations = new();

        /* Játékos létrehozása */
        static PlayerStats player1 = new();

        /* Játékos hozzáadása */
        static bool isMultiplayerChechked = false;

        /* Próbálkozások (1dobás 3 újradobás) */
        static int chance = 4;
        static bool alreadyLocked = false;
        static bool reset = false;

        public Yahtzee()
        {
            InitializeComponent();
        }

        /* Dobás gomb */
        private void throwButton_Click(object sender, EventArgs e)
        {
            /* Vége a játéknak check */
            if (!lockedInformations.ContainsValue(true))
            {
                MessageBox.Show("Vége a játéknak");
                return;
            }

            /* Van-e még próbálkozás */
            if (chance == 0)
            {
                MessageBox.Show("Kifogytál a probálkozásokból!");
                return;
            }

            /* Újra lehessen választani */
            alreadyLocked = false;

            /* Levonjuk a próbálkozást */
            chance--;

            /* Vizualizálás */
            p1ChangeCounterLabel.Text = chance.ToString();

            /* Generáljuk újra azokat, amiket a játékos akar */
            player1.GenerateDiceParts(buttonInformations.Values.ToArray());

            /* Megváltoztatni a dobott összegekre */
            p1GeneratedButton1.Text = player1.CurrentDices[0].ToString();
            p1GeneratedButton2.Text = player1.CurrentDices[1].ToString();
            p1GeneratedButton3.Text = player1.CurrentDices[2].ToString();
            p1GeneratedButton4.Text = player1.CurrentDices[3].ToString();
            p1GeneratedButton5.Text = player1.CurrentDices[4].ToString();

            /* Megnézni miket lehet velük csinálni */
            CheckScores(player1.CurrentDices);
        }
        
        /* Kiválasztás vizualizálása (lejjebb megy a gomb) */
        public void possChanging(object sender, EventArgs e)
        {
            /* Jelenlegi gomb, amivel interakcióba léptünk */
            Button current = (Button)sender;

            /* Ha még nem dobtunk, ne lehessen kiválasztani */
            if (current.Text == "" && !reset)
                return;

            /* Kiválasztás kezelés */
            buttonInformations[current] = !buttonInformations[current];

            /* Pozíció változtatás */
            current.Location = new Point(current.Location.X, buttonInformations[current] ? current.Location.Y + 10 : current.Location.Y - 10);
        }

        /* Eredmények vizsgálata */
        private void CheckScores(int[] dices)
        {
            /* Mindegyik gombnak beírjuk a pontok összegét amit tudunk velük szerezni */
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

        /* Kiválasztás */
        private void SaveLock(object sender, EventArgs e)
        {
            /* Ha már lett választva ne engedje */
            if (alreadyLocked)
            {
                ResetGeneratedNumbers();
                return;
            }

            /* Jelenlegi gomb, amivel interakcióba léptünk */
            Button current = (Button)sender;

            /* Ez a gomb többé nem megnyomható */
            current.Enabled = false;

            /* Vizualizálás */
            current.BackColor = Color.White;

            /* A dobott összegek nullázása */
            p1GeneratedButton5.Text = p1GeneratedButton4.Text = p1GeneratedButton3.Text = p1GeneratedButton2.Text = p1GeneratedButton1.Text = "";
           
            /* Visszakapja a 4dobást */
            chance = 4;
            p1ChangeCounterLabel.Text = (chance - 1).ToString();

            /* Ez a gomb már használva lett */
            lockedInformations[current] = false;

            /* Ne használhassunk több gombot */
            alreadyLocked = true;
            ResetGeneratedNumbers();
        }

        /* Könyvtárak feltöltése minden gomb adattal */
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
            /* Nem játkos kattintás */
            reset = true;

            /* A kiválasztott kockákat visszarakni a sima állapotukba (feltolni) */
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