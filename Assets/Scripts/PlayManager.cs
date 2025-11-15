using System;
using Enemies;
using UI;
using UnityEngine;

namespace PlayManager
{
    public class PlayManager : MonoBehaviour
    {
        [SerializeField] private Player.Player player;
        [SerializeField] private EnemyManager enemyManager;

        private Vector2 _screenBounds;
        private Action _onGameExit;
        
        private void Awake()
        {
            Camera mainCamera = Camera.main;
            float halfHeight = mainCamera.orthographicSize;
            float halfWidth = halfHeight * mainCamera.aspect;
            _screenBounds = new Vector2(halfWidth, halfHeight);
        }

        public void StartGame(Action onGameExited)
        {
            _onGameExit = onGameExited;
            player.Init(_screenBounds);
            enemyManager.Init(player.SpawnPosition, player, _screenBounds);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ExitGame();
            }
        }
        
        
        private void ExitGame()
        {
            _onGameExit?.Invoke();
            enemyManager?.Clear();
        }
    }
}
