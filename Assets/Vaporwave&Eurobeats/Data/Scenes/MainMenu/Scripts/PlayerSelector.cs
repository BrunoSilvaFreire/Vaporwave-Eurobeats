using System;
using Rewired;
using Scripts.Characters;
using Scripts.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Data.Scenes.MainMenu.Scripts {
    public class PlayerSelector : MonoBehaviour {
        public Text PlayerLabel;
        public UICharacterPreviewView PreviewView;
        private Player player;
        public int PlayerID;
        private int index;

        private void Start() {
            player = ReInput.players.GetPlayer(PlayerID);
            PlayerLabel.text = $"Player #{PlayerID}";
        }


        private void Update() {
            if (player == null) {
                return;
            }

            var dir = Math.Sign(player.GetAxis("Horizontal"));
            var db = CharacterDatabase.Instance;
            var max = db.Characters.Count;
            index += dir;
            if (index < 0) {
                index += max;
            }

            if (index >= max) {
                index %= max;
            }

            PreviewView.Character = db.Characters[index];
        }
    }
}