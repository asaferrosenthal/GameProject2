using UnityEngine;

namespace Environment
{
    public class SpawnLocation : MonoBehaviour
    {
        [Tooltip("The radius objects can be placed on start and reset around this spawn location.")]
        public float _SpawnRadius = 5f;
        [Tooltip("The minimum space between spawned prefabs and the other members of the environment, " +
                 "with respect to a Spawner's layer mask.")]
        public float _Padding;
    }
}
