
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemies
{
    public static class EnemyHelpers
    {
        public static Vector2[] GenerateSpawnPosition(int numberOfElements, Vector3 playerPos, int spawnAttempts, float distanceFromPlayer, float spacingBetweenEnemies, Vector2 screenBounds)
        {
            Vector2[] spawnedPositions = new Vector2[numberOfElements];
            var spacingBetweenPlayerSqr = distanceFromPlayer * distanceFromPlayer;
            var spacingBetweenEnemiesSqr = spacingBetweenEnemies * spacingBetweenEnemies;

            for (int i = 0; i < numberOfElements; i++)
            {
                for (int j = 0; j < spawnAttempts; j++)
                {
                    Vector2 pos = new Vector2(
                        Random.Range(-screenBounds.x, screenBounds.x),
                        Random.Range(-screenBounds.y, screenBounds.y)
                    );

                    if (IsInRange(pos, playerPos, spacingBetweenPlayerSqr))
                        continue;

                    bool tooClose = TooCloseToOtherElements(spawnedPositions, pos, spacingBetweenEnemiesSqr, i);
                    if (tooClose) 
                    {
                        if (j < spawnAttempts - 1)
                            continue;
                    }

                    spawnedPositions[i] = pos;
                    break;
                }
            }

            return spawnedPositions;
        }
        
        public static bool IsInRange(Vector2 position, Vector2 otherPosition, float spacingSqr)
        {
            if ((position- otherPosition).sqrMagnitude < spacingSqr)
            {
                return true;
            }
            return false;
        }

        private static bool TooCloseToOtherElements(Vector2[] spawnedPositions, Vector2 position, float spacingBetweenEnemiesSqr, int spawnedIndex)
        {
            for (int i = 0; i < spawnedIndex; i++)
            {
                if (IsInRange(position, spawnedPositions[i], spacingBetweenEnemiesSqr))
                {
                    return true;
                }
            }
            
            return false;
        }

    }
}
