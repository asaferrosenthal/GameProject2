using System;
using System.Collections.Generic;
using UnityEngine;

namespace Environment
{
    public class Spawner : MonoBehaviour
    {
        [Header("Spawning Settings")]
        [Tooltip("List of objects that are spawned in by this spawner.")]
        public List<GameObject> _Prefabs;
        [Tooltip("Number of objects to instantiate with respect to the prefabs list.")]
        public List<int> _SpawnQuantities;
        [Tooltip("The minimum space between spawned prefabs and the environment.")]
        public float _PositionPadding;

        private List<GameObject> _objectlist;
        // This array will be defined by any object that has this object as a parent.
        // These child objects will be used for creating spawn locations
        private SpawnLocation[] _spawnLocations;
        
        /// <summary>
        /// Spawner's layer mask defines which layers must respect the _PositionPadding attribute
        /// </summary>
        private const int LayerMask = (1 << 9) | (1 << 10) | (1 << 11); // Wall layer, Player layer, Adversary layer respectively

        private void Awake()
        {
            // Initialize the array of spawn locations
            _spawnLocations = GetComponentsInChildren<SpawnLocation>();
            
            // Check if the provided spawn data is valid
            if (_Prefabs.Count != _SpawnQuantities.Count)
            {
                Debug.Log("ERROR: There must be a quantity for each prefab specified.");
                throw new Exception("Aborted spawning from : " + this.name);
            }
            
            // Iterate through each required prefab
            // Initialize quantified objects
            for (int i = 0; i < _Prefabs.Count; i++)
            {
                // Instantiate requirement of a given prefab type
                for (int k = 0; k < _SpawnQuantities[i]; k++)
                {
                    _objectlist.Add(Instantiate(_Prefabs[i], transform));
                }
                
            }
        }
    }
}
