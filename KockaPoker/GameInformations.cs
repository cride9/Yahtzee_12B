using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KockaPoker
{
    public class GameInformations
    {
        /* Gomb információk tárolására */
        public static Dictionary<Button, bool>[] buttonInformations = new Dictionary<Button, bool>[] { new Dictionary<Button, bool>(), new Dictionary<Button, bool>() };
        public static Dictionary<Button, bool>[] lockedInformations = new Dictionary<Button, bool>[] { new Dictionary<Button, bool>(), new Dictionary<Button, bool>() };

        /* Játékos létrehozása */
        public static PlayerStats[] players = new PlayerStats[] { new PlayerStats(), new PlayerStats() };

        /* Pont gombok lekérése */
        public Dictionary<Button, bool> GetButtons(playerID ID) =>
            buttonInformations[(int)ID];

        /* Megtartott dobások lekérése */
        public Dictionary<Button, bool> GetLockInformations(playerID ID) =>
            lockedInformations[(int)ID];

        /* A playert magát lekérni */
        public PlayerStats GetPlayer(playerID ID) =>
            players[(int)ID];
    }
}
