using Rewired;
using UnityEngine;

namespace Data.Scenes.MainMenu.Scripts {
    public class LobbyManager : MonoBehaviour {
        public PlayerSelector SelectorPrefab;

        private void Update() {
            if (!ReInput.isReady) {
                return;
            }

            AssignJoysticksToPlayers();
        }

        private void AssignJoysticksToPlayers() {
            // Check all joysticks for a button press and assign it tp
            // the first Player foudn without a joystick
            var joysticks = ReInput.controllers.Joysticks;
            foreach (var joystick in joysticks) {
                if (ReInput.controllers.IsControllerAssigned(joystick.type, joystick.id))
                    continue; // joystick is already assigned to a Player

                // Chec if a button was pressed on the joystick
                if (joystick.GetAnyButtonDown()) {
                    // Find the next Player without a Joystick
                    var player = FindPlayerWithoutJoystick();
                    if (player == null) {
                        return; // no free joysticks
                    }

                    // Assign the joystick to this Player
                    Assing(joystick, player);
                }
            }

            // If all players have joysticks, enable joystick auto-assignment
            // so controllers are re-assigned correctly when a joystick is disconnected
            // and re-connected and disable this script
            if (DoAllPlayersHaveJoysticks()) {
                ReInput.configuration.autoAssignJoysticks = true;
                this.enabled = false; // disable this script
            }
        }

        private void Assing(Joystick joystick, Player player) {
            player.controllers.AddController(joystick, false);
        }

        // Searches all Players to find the next Player without a Joystick assigned
        private Player FindPlayerWithoutJoystick() {
            var players = ReInput.players.Players;
            for (var i = 0; i < players.Count; i++) {
                if (players[i].controllers.joystickCount > 0) continue;
                return players[i];
            }

            return null;
        }

        private bool DoAllPlayersHaveJoysticks() {
            return FindPlayerWithoutJoystick() == null;
        }
    }
}