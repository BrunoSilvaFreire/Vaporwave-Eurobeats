using Rewired;
using Scripts.Characters;
using Scripts.UI;
using UnityEngine;
using UnityEngine.UI;
using UnityUtilities;

namespace Data.Scenes.MainMenu.Scripts {
    public class PlayerSelector : MonoBehaviour {
        public Text PlayerLabel;
        public UICharacterPreviewView PreviewView;
        public Image PlayerStatusIndication;
        public Color InactiveColor = Color.red;
        public Color ActiveColor = Color.green;

        private Player player;
        public byte PlayerID;
        private int index;
        public string LeftKey = "SelectLeft";
        public string RightKey = "SelectRight";

        private void Start() {
            player = ReInput.players.GetPlayer(PlayerID);
            PlayerLabel.text = $"Player #{PlayerID}";
        }


        private void Update() {
            if (player == null) {
                return;
            }

            UpdateIndicatorColor();
            var db = CharacterDatabase.Instance;
            var max = db.Characters.Count;
            if (player.GetButtonDown(LeftKey)) {
                index--;
            }

            if (player.GetButtonDown(RightKey)) {
                index++;
            }

            if (index < 0) {
                index += max;
            }

            if (index >= max) {
                index %= max;
            }

            var c = db.Characters[index];
            PreviewView.Character = c;
            PlayerLabel.color = c.UIColor;
        }

        public bool IsValid() {
            return !player.controllers.Joysticks.IsEmpty();
        }

        private void UpdateIndicatorColor() {
            PlayerStatusIndication.color = IsValid() ? ActiveColor : InactiveColor;
        }
    }
}