using UnityEngine;

namespace Enemies
{
    public struct EnemyData
    {
        public Enemy EnemyObject;
        public Vector3 Direction;
        public bool IsFrozen;
        public bool IgnorePlayerDirection;
        public float IgnorePlayerTimer; 

        public EnemyData(Enemy enemyObject, Vector3 direction, bool isFrozen)
        {
            EnemyObject = enemyObject;
            Direction = direction;
            IgnorePlayerDirection = false;
            IsFrozen = isFrozen;
            IgnorePlayerTimer = 0;
        }
    }
}
