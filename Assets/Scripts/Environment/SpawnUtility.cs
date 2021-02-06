using System;
using UnityEngine;

namespace Environment
{
    /// <summary>
    /// Spawn utility is meant to be a mod-able utility script. If we wish to try different methods of
    /// finding suitable spawn locations do so here. Every method should return a valid vector3.
    /// Examples of further functionality is a boolean determining if we should take the y axis into account.
    /// 
    /// </summary>
    public static class SpawnUtility
    {
        // Hard limit on how many attempts can be made to find a suitable spawn location
        private const int MaxAttempts = 25;

        /// <summary>
        /// Returns a safe spawn position relative to a target
        /// This method only searches for collisions on designated layerMask.
        /// </summary>
        /// <returns></returns>
        public static Vector3 FindSpawnNearTarget(SpawnLocation[] targets, int layerMask, bool yAxis)
        {
            // Initialize return
            Vector3 potentialPosition = Vector3.zero;
            // local counter used to determine if we should kill the process given the @MaxAttempts
            int attemptCount = 0;
            // local bool that tracks if a location has been found
            bool positionFound = false;
                
            // Test new spawn locations until a position is found or we exhaust our maximum number of attempts
            while (!positionFound || attemptCount < MaxAttempts)
            {
                // Select a random spawn location
                int randomIndex = UnityEngine.Random.Range(0, targets.Length);
                SpawnLocation target = targets[randomIndex];
                
                // Create new vector to test
                potentialPosition = target.transform.position + (Vector3.right * UnityEngine.Random.Range(-target._SpawnRadius, target._SpawnRadius)) // x axis
                                                              + (Vector3.forward * UnityEngine.Random.Range(-target._SpawnRadius, target._SpawnRadius)) // z axis
                                                              + (Vector3.up * ( yAxis ? UnityEngine.Random.Range(-target._SpawnRadius, target._SpawnRadius) : 0)); // y axis
                // Test the potential vector
                positionFound = Physics.OverlapSphere(potentialPosition, target._Padding, layerMask).Length == 0;
                attemptCount++;
            }

            if (!positionFound) throw new Exception("A suitable spawn location could not be found, consider repositioning spawn locations and reducing the quantity of what is being spawned");
            
            Debug.Log(potentialPosition);
            
            // Return a proven vector
            return potentialPosition;
        }
    }
}
