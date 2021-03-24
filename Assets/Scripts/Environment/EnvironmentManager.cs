using System;
using Adversary;
using Traps;
using RunMan;
using UnityEngine;

namespace Environment
{
    public class EnvironmentManager : MonoBehaviour
    {
        public TheRunMan _RunMan;
        public bool _Training;
        
        private Spawner[] _spawners;
        private Trap[] _traps;

        private bool _isGamePaused = false;
        
        // Triggers for resetting level
        private float _timeSinceStopped = 0;
        private float _maxTimeSinceStopped = 1f;

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

            if(_RunMan != null) _RunMan.ResetRunMan();
            _timeSinceStopped = 0;
        }

        public void TogglePauseGame()
        {
            _isGamePaused = !_isGamePaused;
            Time.timeScale = _isGamePaused ? 0 : 1;
        }

    }
}
