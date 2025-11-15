using System;
using UnityEngine;

namespace Enemies
{
    public class EnemyManager : MonoBehaviour
    {
        [SerializeField] private Enemy enemyPrefab;
        [SerializeField] private float enemySpeed;
        [SerializeField] private float ignoreDirectionTime = 0.5f;
        [SerializeField] private float randomDirectionFactor = 0.3f;
        
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
                
                //checking if should ignore player still or if timer run out
                if (_enemies[i].IgnorePlayerDirection)
                {
                    _enemies[i].IgnorePlayerTimer -= deltaTime;
                    if (_enemies[i].IgnorePlayerTimer <= 0f)
                        _enemies[i].IgnorePlayerDirection = false;
                }
                
                //checking if should ignore player as it hit wall before
                if (!_enemies[i].IgnorePlayerDirection)
                {
                    Vector2 away = (Vector2)_enemies[i].EnemyObject.transform.position - _playerPosition;
                    _enemies[i].Direction = Vector2.Lerp(_enemies[i].Direction, away.normalized, 0.05f);
                }

                Vector2 pos = _enemies[i].EnemyObject.transform.position + _enemies[i].Direction * (enemySpeed * deltaTime);
                
                //checking for hitting wall
                bool hitWall = false;
                if (pos.x < -_screenBounds.x || pos.x > _screenBounds.x)
                {
                    _enemies[i].Direction.x = -_enemies[i].Direction.x;
                    pos.x = Mathf.Clamp(pos.x, -_screenBounds.x, _screenBounds.x);
                    hitWall = true;
                }
                if (pos.y < -_screenBounds.y || pos.y > _screenBounds.y)
                {
                    _enemies[i].Direction.y = -_enemies[i].Direction.y;
                    pos.y = Mathf.Clamp(pos.y, -_screenBounds.y, _screenBounds.y);
                    hitWall = true;
                }

                //if hitting wall we want for them to ignore player for a bit for a better play
                if (hitWall)
                {
                    _enemies[i].IgnorePlayerDirection = true;
                    _enemies[i].IgnorePlayerTimer = UnityEngine.Random.value * ignoreDirectionTime;
                    _enemies[i].Direction = UnityEngine.Random.insideUnitCircle.normalized;
                }
                
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
                
                if (_enemies[i].IgnorePlayerDirection) continue;
                
                Vector2 baseDir = (Vector2)_enemies[i].EnemyObject.transform.position - playerPos;
                baseDir.Normalize();

                Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * randomDirectionFactor; 

                Vector2 finalDir = (baseDir + randomOffset).normalized;
                
                _enemies[i].Direction = finalDir.normalized;
            }
        }
    }
}
