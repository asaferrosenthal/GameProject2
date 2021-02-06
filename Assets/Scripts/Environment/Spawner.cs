using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Barracuda;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Environment
{
    public class Spawner : MonoBehaviour
    {
        [Header("Spawning Settings")]
        [Tooltip("List of objects that are spawned in by this spawner.")]
        public List<GameObject> _Prefabs;
        [Tooltip("Number of objects to instantiate with respect to the prefabs list.")]
        public List<int> _SpawnQuantities;
        [Tooltip("Toggle randomizing the y axis position, relative to the spawn locations")]
        public bool _ConsiderYAxis;
        
        private List<GameObject> _objectList = new List<GameObject>();
        // This array will be defined by any object that has this object as a parent.
        // These child objects will be used for creating spawn locations
        private SpawnLocation[] _spawnLocations;
        
        /// <summary>
        /// Spawner's layer mask defines which layers must avoid spawning too close to
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
                    // Instantiate and add to the object list
                    GameObject newObject = Instantiate(_Prefabs[i], transform);
                    _objectList.Add(newObject);
                    // Position the new object
                    newObject.transform.position = SpawnUtility.FindSpawnNearTarget(_spawnLocations, LayerMask, _ConsiderYAxis);
                }
            }
        }

        /// <summary>
        /// Method to be used on runtime to reset the spawn positions of level objects specified by this specific spawner.
        /// </summary>
        public void ResetObjects()
        {
            // Resets the positions of spawned in objects
            // TO DO: If objects have specific resets we can;
            // have a manager for relevant objects that resets the objects data and then calls this spawner... or <--- probably this one
            // create a subscription that notifies objects of this method call ... or
            // specify types the spawner control and directly call resets on this method... or
            foreach (GameObject ele in _objectList)
            {
                ele.transform.position = SpawnUtility.FindSpawnNearTarget(_spawnLocations, LayerMask, _ConsiderYAxis);
            }
        }
        
    }
}
