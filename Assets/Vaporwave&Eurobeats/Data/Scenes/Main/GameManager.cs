using System.Collections.Generic;
using Scripts.Game;
using Scripts.World;
using UnityEngine;
using UnityUtilities;

namespace Data.Scenes.Main {
    public class GameManager : MonoBehaviour {
        public MovableEntity Prefab;
        private void Start() {
            var players = GameData.ActivePlayers;
            if (players == null) {
                InitEmpty();
            } else {
                Init(players);
            }

            CameraController.Instance.SearchTargets();
        }

        private void InitEmpty() {
            var world = World.Instance;
            var position = new Vector2(Random.value, Random.value);
            var spawnPoint = world.ToSpawnPoint(position);
            Prefab.Clone(spawnPoint);
        }

        private void Init(IEnumerable<PlayerData> players) {
            foreach (var playerData in players) {
                Debug.Log($"Loading player {playerData}");
                SpawnPlayer(playerData);
            }
        }

        private void SpawnPlayer(PlayerData playerData) {
            var world = World.Instance;
            var position = new Vector2(Random.value, Random.value);
            var spawnPoint = world.ToSpawnPoint(position);
            var entity = Prefab.Clone(spawnPoint);
            entity.PlayerNumber = playerData.Player;
            entity.LoadCharacter(playerData.Character);
        }
    }
}