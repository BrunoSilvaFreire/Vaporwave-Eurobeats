using System;
using Scripts.Characters;

namespace Scripts.Game {
    [Serializable]
    public class PlayerData {
        public byte Player;
        public Character Character;

        public PlayerData(byte player, Character character) {
            Player = player;
            Character = character;
        }

        public override string ToString() {
            return $"{nameof(Player)}: {Player}, {nameof(Character)}: {Character}";
        }
    }

    public static class GameData {
        public static PlayerData[] ActivePlayers;
    }
}