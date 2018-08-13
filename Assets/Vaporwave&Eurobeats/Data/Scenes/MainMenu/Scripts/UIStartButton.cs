using System.Linq;
using Rewired;
using UnityEngine;
using UnityEngine.UI;

namespace Data.Scenes.MainMenu.Scripts {
    public class UIStartButton : MonoBehaviour {
        public Button Button;
        public LobbyManager Manager;
        public GameObject[] SubChildren;

        private void Update() {
            var ready = Manager.PlayerSelectors.Count(player => player.IsValid());
            var valid = ready > 1;
            Button.interactable = valid;
            foreach (var subChild in SubChildren) {
                subChild.SetActive(!valid);
            }

            if (valid && PressedStart()) {
                Manager.StartGame();
            }
        }

        private static bool PressedStart() {
            return ReInput.players.Players.Any(player => player.GetButtonDown("Start"));
        }
    }
}