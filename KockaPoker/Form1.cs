using System;
using System.Runtime.CompilerServices;
using System.Threading.Channels;
using static System.Formats.Asn1.AsnWriter;

namespace KockaPoker
{
    public enum playerID : int
    {
        PLAYER1,
        PLAYER2
    }

    public partial class Yahtzee : Form
    {

        /* ArtificalPlayer */
        static ArtificalOpponent opponent;

        /* Információk kezelése */
        static GameInformations info = new();

        /* Játékos hozzáadása */
        static bool isMultiplayerChechked = false;

        /* Próbálkozások (1dobás 3 újradobás) */
        static int chance = 4;
        static bool alreadyLocked = false;
        static bool reset = false;

        static playerID playerTurn = playerID.PLAYER1;

        /* Players has Bonus? */
        static bool p1HasBonus = false;
        static bool p2HasBonus = false;

        /* Players Summarys */
        static int p1Summary = 0;
        static int p2Summary = 0;

        public Yahtzee()
        {
            InitializeComponent();
        }

        /* Dobás gomb */
        private void Dobas(object sender, EventArgs e)
        {
            if (playerTurn == playerID.PLAYER1 && (Button)sender == player1ThrowButton)
            {
                multiplayerCheck.Hide();
                if (!info.GetLockInformations(playerID.PLAYER1).ContainsValue(true))
                {
                    //p1FirstSum.Text = TopSideSum(playerID.PLAYER1).ToString();

                    //if (p1HasBonus)
                    //    p1FirstBonus.Text = "25";
                    //else
                    //    p1FirstBonus.Text = "0";

                    //BotSideSum(playerID.PLAYER1);
                    //p1Sum.Text = p1Summary.ToString();

                    multiplayerCheck.Show();
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
                player1ChangeCounterLabel.Text = chance.ToString();

                /* Generáljuk újra azokat, amiket a játékos akar */
                info.GetPlayer(playerID.PLAYER1).GenerateDiceParts(info.GetButtons(playerID.PLAYER1).Values.ToArray());

                /* Megváltoztatni a dobott összegekre */
                p1GeneratedButton1.Text = info.GetPlayer(playerID.PLAYER1).CurrentDices[0].ToString();
                p1GeneratedButton2.Text = info.GetPlayer(playerID.PLAYER1).CurrentDices[1].ToString();
                p1GeneratedButton3.Text = info.GetPlayer(playerID.PLAYER1).CurrentDices[2].ToString();
                p1GeneratedButton4.Text = info.GetPlayer(playerID.PLAYER1).CurrentDices[3].ToString();
                p1GeneratedButton5.Text = info.GetPlayer(playerID.PLAYER1).CurrentDices[4].ToString();

                /* Megnézni miket lehet velük csinálni */
                CheckScores(info.GetPlayer(playerID.PLAYER1).CurrentDices);
                info.GetPlayer(playerID.PLAYER1).ThrowChance = chance;
            }
            else if (playerTurn == playerID.PLAYER2 && (Button)sender == player2ThrowButton)
            {
                if (!info.GetLockInformations(playerID.PLAYER2).ContainsValue(true))
                {
                    //p2FirstSum.Text = TopSideSum(playerID.PLAYER2).ToString();

                    //if (p2HasBonus)
                    //    p2FirstBonus.Text = "25";
                    //else
                    //    p2FirstBonus.Text = "0";

                    //BotSideSum(playerID.PLAYER2);
                    //p2Sum.Text = p2Summary.ToString();

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
                player2ChangeCounterLabel.Text = chance.ToString();

                /* Generáljuk újra azokat, amiket a játékos akar */
                info.GetPlayer(playerID.PLAYER2).GenerateDiceParts(info.GetButtons(playerID.PLAYER2).Values.ToArray());

                /* Megváltoztatni a dobott összegekre */
                p2GeneratedButton1.Text = info.GetPlayer(playerID.PLAYER2).CurrentDices[0].ToString();
                p2GeneratedButton2.Text = info.GetPlayer(playerID.PLAYER2).CurrentDices[1].ToString();
                p2GeneratedButton3.Text = info.GetPlayer(playerID.PLAYER2).CurrentDices[2].ToString();
                p2GeneratedButton4.Text = info.GetPlayer(playerID.PLAYER2).CurrentDices[3].ToString();
                p2GeneratedButton5.Text = info.GetPlayer(playerID.PLAYER2).CurrentDices[4].ToString();

                /* Megnézni miket lehet velük csinálni */
                CheckScores(info.GetPlayer(playerID.PLAYER2).CurrentDices);
                info.GetPlayer(playerID.PLAYER2).ThrowChance = chance;
            }
        }

        /* Kiválasztás vizualizálása (lejjebb megy a gomb) */
        public void PossChanging(object sender, EventArgs e)
        {
            /* Jelenlegi gomb, amivel interakcióba léptünk */
            Button current = (Button)sender;

            /* Ha még nem dobtunk, ne lehessen kiválasztani */
            if (current.Text == "" && !reset)
                return;

            if (playerTurn == playerID.PLAYER1)
            {
                /* Kiválasztás kezelés */
                info.GetButtons(playerID.PLAYER1)[current] = !info.GetButtons(playerID.PLAYER1)[current];

                /* Pozíció változtatás */
                current.Location = new Point(current.Location.X, info.GetButtons(playerID.PLAYER1)[current] ? current.Location.Y + 10 : current.Location.Y - 10);
            }
            else
            {
                /* Kiválasztás kezelés */
                info.GetButtons(playerID.PLAYER2)[current] = !info.GetButtons(playerID.PLAYER2)[current];

                /* Pozíció változtatás */
                current.Location = new Point(current.Location.X, info.GetButtons(playerID.PLAYER2)[current] ? current.Location.Y + 10 : current.Location.Y - 10);

            }
        }

        /* Eredmények vizsgálata */
        private void CheckScores(int[] dices)
        {
            /* Mindegyik gombnak beírjuk a pontok összegét amit tudunk velük szerezni */
            if (playerTurn == playerID.PLAYER1)
            {
                p1Ones.Text = info.GetLockInformations(playerID.PLAYER1)[p1Ones] ? info.GetPlayer(playerID.PLAYER1).ScoreNumbers(dices, 1).ToString() : p1Ones.Text;
                p1Twos.Text = info.GetLockInformations(playerID.PLAYER1)[p1Twos] ? info.GetPlayer(playerID.PLAYER1).ScoreNumbers(dices, 2).ToString() : p1Twos.Text;
                p1Threes.Text = info.GetLockInformations(playerID.PLAYER1)[p1Threes] ? info.GetPlayer(playerID.PLAYER1).ScoreNumbers(dices, 3).ToString() : p1Threes.Text;
                p1Fours.Text = info.GetLockInformations(playerID.PLAYER1)[p1Fours] ? info.GetPlayer(playerID.PLAYER1).ScoreNumbers(dices, 4).ToString() : p1Fours.Text;
                p1Fives.Text = info.GetLockInformations(playerID.PLAYER1)[p1Fives] ? info.GetPlayer(playerID.PLAYER1).ScoreNumbers(dices, 5).ToString() : p1Fives.Text;
                p1Sixes.Text = info.GetLockInformations(playerID.PLAYER1)[p1Sixes] ? info.GetPlayer(playerID.PLAYER1).ScoreNumbers(dices, 6).ToString() : p1Sixes.Text;

                p1ThreeKind.Text = info.GetLockInformations(playerID.PLAYER1)[p1ThreeKind] ? info.GetPlayer(playerID.PLAYER1).ScoreKinds(dices, 3).ToString() : p1ThreeKind.Text;
                p1FourKind.Text = info.GetLockInformations(playerID.PLAYER1)[p1FourKind] ? info.GetPlayer(playerID.PLAYER1).ScoreKinds(dices, 4).ToString() : p1FourKind.Text;
                p1FullHouse.Text = info.GetLockInformations(playerID.PLAYER1)[p1FullHouse] ? info.GetPlayer(playerID.PLAYER1).ScoreFullHouse(dices).ToString() : p1FullHouse.Text;
                p1SmallStraight.Text = info.GetLockInformations(playerID.PLAYER1)[p1SmallStraight] ? info.GetPlayer(playerID.PLAYER1).ScoreStraight(dices, 4).ToString() : p1SmallStraight.Text;
                p1LargeStraight.Text = info.GetLockInformations(playerID.PLAYER1)[p1LargeStraight] ? info.GetPlayer(playerID.PLAYER1).ScoreStraight(dices, 5).ToString() : p1LargeStraight.Text;
                p1Yahtzee.Text = info.GetLockInformations(playerID.PLAYER1)[p1Yahtzee] ? info.GetPlayer(playerID.PLAYER1).ScoreYahtzee(dices).ToString() : p1Yahtzee.Text;
                p1Chance.Text = info.GetLockInformations(playerID.PLAYER1)[p1Chance] ? info.GetPlayer(playerID.PLAYER1).ScoreChance(dices).ToString() : p1Chance.Text;

            }
            else if (playerTurn == playerID.PLAYER2)
            {
                p2Ones.Text = info.GetLockInformations(playerID.PLAYER2)[p2Ones] ? info.GetPlayer(playerID.PLAYER2).ScoreNumbers(dices, 1).ToString() : p2Ones.Text;
                p2Twos.Text = info.GetLockInformations(playerID.PLAYER2)[p2Twos] ? info.GetPlayer(playerID.PLAYER2).ScoreNumbers(dices, 2).ToString() : p2Twos.Text;
                p2Threes.Text = info.GetLockInformations(playerID.PLAYER2)[p2Threes] ? info.GetPlayer(playerID.PLAYER2).ScoreNumbers(dices, 3).ToString() : p2Threes.Text;
                p2Fours.Text = info.GetLockInformations(playerID.PLAYER2)[p2Fours] ? info.GetPlayer(playerID.PLAYER2).ScoreNumbers(dices, 4).ToString() : p2Fours.Text;
                p2Fives.Text = info.GetLockInformations(playerID.PLAYER2)[p2Fives] ? info.GetPlayer(playerID.PLAYER2).ScoreNumbers(dices, 5).ToString() : p2Fives.Text;
                p2Sixes.Text = info.GetLockInformations(playerID.PLAYER2)[p2Sixes] ? info.GetPlayer(playerID.PLAYER2).ScoreNumbers(dices, 6).ToString() : p2Sixes.Text;

                p2ThreeKind.Text = info.GetLockInformations(playerID.PLAYER2)[p2ThreeKind] ? info.GetPlayer(playerID.PLAYER2).ScoreKinds(dices, 3).ToString() : p2ThreeKind.Text;
                p2FourKind.Text = info.GetLockInformations(playerID.PLAYER2)[p2FourKind] ? info.GetPlayer(playerID.PLAYER2).ScoreKinds(dices, 4).ToString() : p2FourKind.Text;
                p2FullHouse.Text = info.GetLockInformations(playerID.PLAYER2)[p2FullHouse] ? info.GetPlayer(playerID.PLAYER2).ScoreFullHouse(dices).ToString() : p2FullHouse.Text;
                p2SmallStraight.Text = info.GetLockInformations(playerID.PLAYER2)[p2SmallStraight] ? info.GetPlayer(playerID.PLAYER2).ScoreStraight(dices, 4).ToString() : p2SmallStraight.Text;
                p2LargeStraight.Text = info.GetLockInformations(playerID.PLAYER2)[p2LargeStraight] ? info.GetPlayer(playerID.PLAYER2).ScoreStraight(dices, 5).ToString() : p2LargeStraight.Text;
                p2Yahtzee.Text = info.GetLockInformations(playerID.PLAYER2)[p2Yahtzee] ? info.GetPlayer(playerID.PLAYER2).ScoreYahtzee(dices).ToString() : p2Yahtzee.Text;
                p2Chance.Text = info.GetLockInformations(playerID.PLAYER2)[p2Chance] ? info.GetPlayer(playerID.PLAYER2).ScoreChance(dices).ToString() : p2Chance.Text;

            }
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

            if (sender.GetType() != typeof(Button))
                return;

            /* Jelenlegi gomb, amivel interakcióba léptünk */
            Button current = (Button)sender;

            /* ha ures a gomb ne nyomjon mar ra wtf */
            if (current.Text == "")
            {
                return;
            }
      
            /* Jelenlegi player check */
            if (playerTurn == playerID.PLAYER1)
            {
                /* Biztos sajátjára kattintott-e */
                foreach (var currentButton in info.GetLockInformations(playerID.PLAYER1))
                {
                    /* Saját gombja */
                    if (current == currentButton.Key)
                    {
                        /* Ez a gomb többé nem megnyomható */
                        current.Enabled = false;

                        /* Vizualizálás */
                        current.BackColor = Color.White;

                        /* A dobott összegek nullázása */
                        p1GeneratedButton5.Text = p1GeneratedButton4.Text = p1GeneratedButton3.Text = p1GeneratedButton1.Text = p1GeneratedButton2.Text = "";

                        /* Ez a gomb már használva lett */
                        info.GetLockInformations(playerID.PLAYER1)[current] = false;

                        /* Visszakapja a 4dobást */
                        chance = 4;
                        info.GetPlayer(playerID.PLAYER1).ThrowChance = chance;

                        /* Lehetoségek */
                        player1ChangeCounterLabel.Text = (chance - 1).ToString();

                        /* Ne használhassunk több gombot */
                        alreadyLocked = true;
                        ResetGeneratedNumbers();

                        if (isMultiplayerChechked) 
                        { 
                            playerTurn = playerID.PLAYER2;
                            player1ThrowButton.Hide();
                            player2ThrowButton.Show();

                            opponent = new(info, playerID.PLAYER2, this);
                        }
                    }
                }
                if (!info.GetLockInformations(playerID.PLAYER1).ContainsValue(true)) {
                    p1FirstSum.Text = TopSideSum(playerID.PLAYER1).ToString();

                    if (p1HasBonus)
                        p1FirstBonus.Text = "25";
                    else
                        p1FirstBonus.Text = "0";

                    BotSideSum(playerID.PLAYER1);
                    p1Sum.Text = p1Summary.ToString();

                    return;
                }
            }
            else
            {
                /* Biztos sajátjára kattintott-e */
                foreach (var currentButton in info.GetLockInformations(playerID.PLAYER2))
                {
                    /* Saját gombja */
                    if (current == currentButton.Key)
                    {
                        /* Ez a gomb többé nem megnyomható */
                        current.Enabled = false;

                        /* Vizualizálás */
                        current.BackColor = Color.White;

                        /* A dobott összegek nullázása */
                        p2GeneratedButton5.Text = p2GeneratedButton4.Text = p2GeneratedButton3.Text = p2GeneratedButton1.Text = p2GeneratedButton2.Text = "";

                        /* Ez a gomb már használva lett */
                        info.GetLockInformations(playerID.PLAYER2)[current] = false;

                        /* Visszakapja a 4dobást */
                        chance = 4;
                        info.GetPlayer(playerID.PLAYER2).ThrowChance = chance;

                        /* Lehetoségek */
                        player2ChangeCounterLabel.Text = (chance - 1).ToString();

                        /* Ne használhassunk több gombot */
                        alreadyLocked = true;
                        ResetGeneratedNumbers();

                        if (isMultiplayerChechked)
                        {
                            playerTurn = playerID.PLAYER1;
                            player1ThrowButton.Show();
                            player2ThrowButton.Hide();
                        }
                    }
                }

                if (!info.GetLockInformations(playerID.PLAYER2).ContainsValue(true)) {
                    p2FirstSum.Text = TopSideSum(playerID.PLAYER2).ToString();

                    if (p2HasBonus)
                        p2FirstBonus.Text = "25";
                    else
                        p2FirstBonus.Text = "0";

                    BotSideSum(playerID.PLAYER2);
                    p2Sum.Text = p2Summary.ToString();

                    return;
                }
            }
        }

        /* Könyvtárak feltöltése minden gomb adattal */
        private void Yahtzee_Load(object sender, EventArgs e)
        {
            /* Azért ez a megoldás és nem egyenkénti beírogatás, mert késobb lehet változtatni */

            /* Összes player információt megszerezni */
            for (int i = 0; i < Controls.Count; i++)
            {
                /* Felso rész = mindketto player check */
                if (Controls[i] == topPanel)
                {
                    foreach (Control currentItem in topPanel.Controls)
                    {
                        if (currentItem.Name.StartsWith("p1"))
                            info.GetLockInformations(playerID.PLAYER1).Add((Button)currentItem, true);
                        else if (currentItem.Name.StartsWith("p2"))
                            info.GetLockInformations(playerID.PLAYER2).Add((Button)currentItem, true);
                    }
                }
                /* Also rész = mindketto player check */
                else if (Controls[i] == botPanel)
                {
                    foreach (Control currentItem in botPanel.Controls)
                    {
                        if (currentItem.Name.StartsWith("p1"))
                            info.GetLockInformations(playerID.PLAYER1).Add((Button)currentItem, true);
                        else if (currentItem.Name.StartsWith("p2"))
                            info.GetLockInformations(playerID.PLAYER2).Add((Button)currentItem, true);
                    }
                }
                /* Csak player1 check a kockákhoz */
                else if (Controls[i] == player1Panel)
                {
                    foreach (Control player1Item in player1Panel.Controls)
                        if (player1Item.Name.StartsWith("p1Gen"))
                            info.GetButtons(playerID.PLAYER1).Add((Button)player1Item, false);
                }
                /* Csak player2 check a kockákhoz */
                else if (Controls[i] == player2Panel)
                {
                    foreach (Control player2Item in player2Panel.Controls)
                        if (player2Item.Name.StartsWith("p2Gen"))
                            info.GetButtons(playerID.PLAYER2).Add((Button)player2Item, false);
                }
            }
            player1ThrowButton.Show();
            player2ThrowButton.Hide();
        }

        void ResetGeneratedNumbers()
        {
            /* Nem játkos kattintás */
            reset = true;

            /* A kiválasztott kockákat visszarakni a sima állapotukba (feltolni) */
            foreach (var item in info.GetButtons(playerID.PLAYER1))
                if (info.GetButtons(playerID.PLAYER1)[item.Key])
                    item.Key.PerformClick();

            foreach (var item in info.GetButtons(playerID.PLAYER2))
                if (info.GetButtons(playerID.PLAYER2)[item.Key])
                    item.Key.PerformClick();

            reset = false;
        }

        private void MultiPlayerCheckbox(object sender, EventArgs e)
        {
            if (isMultiplayerChechked)
            {
                player2Panel.Visible = false;
                isMultiplayerChechked = false;

                return;
            }

            player2Panel.Visible = true;
            isMultiplayerChechked = true;
            
        }

        public int TopSideSum(playerID ID)
        {
            /* Player 1 elsõ rész Összegzõ */
            if(ID == playerID.PLAYER1)
            {
                int p1Part1Sum = int.Parse(p1Ones.Text) + int.Parse(p1Twos.Text) + int.Parse(p1Threes.Text) + int.Parse(p1Fours.Text) + int.Parse(p1Fives.Text) + int.Parse(p1Sixes.Text);

                if (p1Part1Sum >= 63)
                    p1HasBonus = true;

                p1Summary += p1Part1Sum;

                return p1Part1Sum;
            }

            /* Player 2 elsõ rész Összegzõ */
            else if (ID == playerID.PLAYER2)
            {
                int p2Part1Sum = int.Parse(p2Ones.Text) + int.Parse(p2Twos.Text) + int.Parse(p2Threes.Text) + int.Parse(p2Fours.Text) + int.Parse(p2Fives.Text) + int.Parse(p2Sixes.Text);

                if (p2Part1Sum >= 63)
                    p2HasBonus = true;

                p2Summary += p2Part1Sum;

                return p2Part1Sum;
            }

            return 0;
        }
        public void BotSideSum(playerID ID)
        {

            /* Player 1 Összegzõ */
            if (ID == playerID.PLAYER1)
            {
                p1Summary += int.Parse(p1ThreeKind.Text) + int.Parse(p1FourKind.Text) + int.Parse(p1FullHouse.Text) + int.Parse(p1SmallStraight.Text) + int.Parse(p1LargeStraight.Text) + int.Parse(p1Yahtzee.Text) + int.Parse(p1Chance.Text);

                if (p1HasBonus)
                    p1Summary += 25;
            }

            /* Player 2 Összegzõ */
            else if (ID == playerID.PLAYER2)
            {
                p2Summary += int.Parse(p2ThreeKind.Text) + int.Parse(p2FourKind.Text) + int.Parse(p2FullHouse.Text) + int.Parse(p2SmallStraight.Text) + int.Parse(p2LargeStraight.Text) + int.Parse(p2Yahtzee.Text) + int.Parse(p2Chance.Text);

                if (p2HasBonus)
                    p2Summary += 25;
            }
        }

        private void ResetGame(object sender, MouseEventArgs e)
        {
            multiplayerCheck.Show();
            ResetPlayer(playerID.PLAYER1);
            ResetPlayer(playerID.PLAYER2);
        }

        private void ResetPlayer(playerID playerId)
        {
            foreach (var button in info.GetButtons(playerId))
            {
                info.GetButtons(playerId)[button.Key] = false;
                button.Key.Text = "";
                button.Key.Enabled = true;
                button.Key.BackColor = Color.FromArgb(25, 25, 25);
            }

            foreach (var button in info.GetLockInformations(playerId))
            {
                info.GetLockInformations(playerId)[button.Key] = true;
                button.Key.Text = "";
                button.Key.Enabled = true;
                button.Key.BackColor = Color.FromArgb(25, 25, 25);
            }

            chance = 4;
            info.GetPlayer(playerId).ThrowChance = chance;
            player1ChangeCounterLabel.Text = (chance - 1).ToString();

            ResetGeneratedNumbers();
        }
    }
}