using System.Collections.Generic;
using UnityEngine;


namespace Environment
{
    public static class RunManTrainingAssistant
    {
        public static Vector3 RandomizeSpawn(List<Transform> spawns, GameObject man)
        {
            int index = Random.Range(0, spawns.Count);
            Vector3 location = spawns[index].position;
            return location;
        }
    }
}
