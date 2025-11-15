using System;
using UnityEngine;

namespace Enemies
{
    public class EnemyManager : MonoBehaviour
    {
        [SerializeField] private Enemy enemyPrefab;
        [SerializeField] private float enemySpeed;
        
        [Header("Spawn Settings")]
        [SerializeField] private int enemyCount = 1000;
        [SerializeField] private float dsitanceFromPlayer = 2f;
        [SerializeField] private float spacingBetweenElements = 0.2f;
        [SerializeField] private int maxSpawnAttemptsPerEnemy = 20;
        
        private EnemyData[] _enemies;
        private Action<Vector2> _onPlayerMovedAction;
        private Vector2 _screenBounds;
        private Vector2 _playerPosition;
        private float _touchDistanceSqr;

        private void OnDestroy()
        {
            if(_onPlayerMovedAction != null)
                _onPlayerMovedAction -= HandleOnPlayerMoved;
        }

        public void Init(Vector2 playerStartingPosition, Player.Player player, Vector2 screenBounds)
        {
            _touchDistanceSqr = (enemyPrefab.gameObject.transform.localScale.x / 2) * (enemyPrefab.gameObject.transform.localScale.x / 2);
            player.OnPlayerMoved += HandleOnPlayerMoved;
            _onPlayerMovedAction += player.OnPlayerMoved;
            _screenBounds = screenBounds;
            _playerPosition =  playerStartingPosition;
            
            _enemies = new EnemyData[enemyCount];

            var generatedPositions = EnemyHelpers.GenerateSpawnPosition(
                enemyCount,
                playerStartingPosition, 
                maxSpawnAttemptsPerEnemy,
                dsitanceFromPlayer, 
                spacingBetweenElements, 
                screenBounds);
            
            for (int i = 0; i < _enemies.Length; i++)
            {
                var enemyObject = Instantiate(enemyPrefab, transform);

                if (enemyObject == null) 
                    continue;
                
                if (i >= generatedPositions.Length)
                    continue;
                
                enemyObject.transform.position = generatedPositions[i];
                enemyObject.SetFreeze(false);

                _enemies[i] = new EnemyData(
                    enemyObject,
                    Vector3.zero,
                    false
                );
            }
            
        }
        
        void Update()
        {
            float deltaTime = Time.deltaTime;

            for (int i = 0; i < _enemies.Length; i++)
            {
                if (_enemies[i].EnemyObject == null) continue;
                
                if (_enemies[i].IsFrozen)
                    continue;
                
                Vector2 pos = _enemies[i].EnemyObject.transform.position + _enemies[i].Direction * (enemySpeed * deltaTime);
                
                pos.x = Mathf.Clamp(pos.x, -_screenBounds.x, _screenBounds.x);
                pos.y = Mathf.Clamp(pos.y, -_screenBounds.y, _screenBounds.y);

                _enemies[i].EnemyObject.transform.position = pos;
                
                if (EnemyHelpers.IsInRange(pos, _playerPosition, _touchDistanceSqr))
                {
                    _enemies[i].IsFrozen = true;
                    if (_enemies[i].EnemyObject != null)
                    {
                        _enemies[i].EnemyObject.SetFreeze(true);
                    }
                }
            }
        }
        
        public void Clear()
        {
            if (_onPlayerMovedAction != null)
                _onPlayerMovedAction -= HandleOnPlayerMoved;

            if (_enemies != null)
            {
                for (int i = 0; i < _enemies.Length; i++)
                {
                    if (_enemies[i].EnemyObject != null)
                        Destroy(_enemies[i].EnemyObject.gameObject);
                }
            }

            _enemies = null;
            _playerPosition = Vector2.zero;
            _onPlayerMovedAction = null;
        }
        
        private void HandleOnPlayerMoved(Vector2 playerPos)
        {
            _playerPosition = playerPos;
            
            for (int i = 0; i < _enemies.Length; i++)
            {
                if (_enemies[i].IsFrozen) continue;
                Vector2 dir = (Vector2)_enemies[i].EnemyObject.transform.position - playerPos;
                _enemies[i].Direction = dir.normalized;
            }
        }
    }
}
