using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Rewired;
using Scripts.Game;
using Scripts.UI;
using Scripts.World;
using Shiroi.FX.Effects;
using Shiroi.FX.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityUtilities;

namespace Data.Scenes.Main {
    public class GameManager : MonoBehaviour {
        public MovableEntity Prefab;
        public float DeathHeight;
        private readonly List<MovableEntity> playerEntities = new List<MovableEntity>();
        private readonly List<MovableEntity> deadEntities = new List<MovableEntity>();
        public WorldEffect DeathEffect;
        public WorldEffect VictoryEffect;
        public bool Playing;
        public UIPanel EndPanel;
        public float WaitDuration;
        public Selectable ToSelectOnEnd;

        private void SetInputMaps(bool ui) {
            foreach (var player in ReInput.players.Players) {
                foreach (var map in player.controllers.maps.GetAllMapsInCategory("UI")) {
                    map.enabled = ui;
                }
            }

            foreach (var player in ReInput.players.Players) {
                foreach (var map in player.controllers.maps.GetAllMapsInCategory("Default")) {
                    map.enabled = !ui;
                }
            }
        }

        private void Update() {
            if (!Playing) {
                return;
            }

            playerEntities.RemoveAll(delegate(MovableEntity entity) {
                var pos = entity.transform.position;
                if (pos.y <= DeathHeight) {
                    entity.gameObject.SetActive(false);
                    deadEntities.Add(entity);
                    DeathEffect.ExecuteIfPresent(pos);
                    CameraController.Instance.SearchTargets();
                    return true;
                }

                return false;
            });
            if (playerEntities.Count == 1) {
                End(playerEntities.First());
            }
        }

        private void End(MovableEntity winner) {
            StartCoroutine(EndRoutine(winner));
        }

        public void GoToLobby() {
            SceneManager.LoadScene(0);
        }

        private IEnumerator EndRoutine(MovableEntity winner) {
            VictoryEffect.ExecuteIfPresent(winner.transform.position);
            Playing = false;
            yield return new WaitForSeconds(WaitDuration);
            EndPanel.Show();
            ToSelectOnEnd.Select();
            //SetInputMaps(true);
        }

        private void Start() {
            EndPanel.SnapShowing(false);
            Restart(false, true);
        }

        private void InitEmpty() {
            var world = World.Instance;
            var position = new Vector2(Random.value, Random.value);
            var spawnPoint = world.ToSpawnPoint(position);
            Prefab.Clone(spawnPoint);
        }

        public void RestartGame() {
            Restart();
        }

        public void Restart(bool regenerate = true, bool reloadPlayer = false) {
            if (regenerate) {
                World.Instance.Generate();
            }

            EndPanel.Hide();
            //SetInputMaps(false);

            var players = GameData.ActivePlayers;
            if (players == null) {
                InitEmpty();
            } else {
                Init(players, reloadPlayer);
            }

            deadEntities.Clear();
            CameraController.Instance.SearchTargets();
            Playing = true;
        }

        private MovableEntity GetPlayerEntity(byte player) {
            foreach (var movable in deadEntities) {
                if (movable.PlayerNumber == player) {
                    return movable;
                }
            }

            foreach (var movable in playerEntities) {
                if (movable.PlayerNumber == player) {
                    return movable;
                }
            }

            return null;
        }

        private void Init(IEnumerable<PlayerData> players, bool reloadPlayer) {
            var newEntities = new List<MovableEntity>();
            foreach (var playerData in players) {
                var world = World.Instance;
                var position = GetPos(playerData.Player);
                var spawnPoint = world.ToSpawnPoint(position);
                MovableEntity entity;
                if (reloadPlayer) {
                    entity = Prefab.Clone(spawnPoint);

                    foreach (var subEntity in entity.GetComponentsInChildren<MovableEntity>()) {
                        subEntity.PlayerNumber = playerData.Player;
                    }

                    entity.PlayerNumber = playerData.Player;
                    entity.LoadCharacter(playerData.Character);
                } else {
                    entity = GetPlayerEntity(playerData.Player);
                    entity.gameObject.SetActive(true);
                    entity.transform.position = spawnPoint;
                }

                var s = entity.State as DudeMoveState;
                if (s != null) {
                    s.CubeStorage = s.MaximumStorage;
                }

                newEntities.Add(entity);
            }

            playerEntities.Clear();
            playerEntities.AddRange(newEntities);
        }

        private Vector2 GetPos(byte playerDataPlayer) {
            switch (playerDataPlayer) {
                case 0:
                    return new Vector2(0.25F, 0.25F);
                case 1:
                    return new Vector2(0.25F, 0.75F);
                case 2:
                    return new Vector2(0.75F, 0.25F);
                case 3:
                    return new Vector2(0.75F, 0.75F);
            }

            return new Vector2(.5F, .5F);
        }
    }
}