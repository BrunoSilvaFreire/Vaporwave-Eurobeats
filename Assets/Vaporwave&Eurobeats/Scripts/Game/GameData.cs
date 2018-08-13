using System;
using Scripts.Characters;

namespace Scripts.Game {
    [Serializable]
    public struct PlayerData {
        public int Player;
        public Character Character;
    }

    public static class GameData {
        public static PlayerData[] ActivePlayers;
    }
}