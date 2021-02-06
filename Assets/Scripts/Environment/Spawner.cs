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
        public int[] _SpawnQuantities;
        [Tooltip("The minimum space between spawned prefabs and the environment.")]
        public float _PositionPadding;
    }
}
