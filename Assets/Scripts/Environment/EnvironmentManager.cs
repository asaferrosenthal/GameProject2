using Traps;
using UnityEngine;

namespace Environment
{
    public class EnvironmentManager : MonoBehaviour
    {
        private Spawner[] _spawners;
        private Trap[] _traps;
        private void Awake()
        {
            _spawners = GetComponentsInChildren<Spawner>();
            _traps = GetComponentsInChildren<Trap>();
        }

        public void ResetEnvironment()
        {
            foreach(Spawner ele in _spawners)
            {
                ele.ResetObjectPositions();
            }

            foreach (Trap ele in _traps)
            {
                ele.ResetTrap();
            }
        }
    }
}
