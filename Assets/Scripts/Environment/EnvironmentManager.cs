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
        
        public DeathField deathfield;

        // Triggers for resetting level
        private float _timeSinceStopped = 0;
        private float _maxTimeSinceStopped = 1f;
        private Rigidbody _rigidbody;

        private void Awake()
        {
            _spawners = GetComponentsInChildren<Spawner>();
            _traps = GetComponentsInChildren<Trap>();
            _rigidbody = _RunMan.GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            CheckIfStopped();
        }

        private void CheckIfStopped()
        {
            if (_Training) return;
            if (Mathf.Floor(_rigidbody.velocity.magnitude) == 0)
            {
                // Add how much time has passed since last check
                _timeSinceStopped += Time.fixedDeltaTime;
                
                // End the game if we reached the time limit for being stopped
                if (deathfield.gone) ResetEnvironment();
            }
            else
            {
                // Reset the time
                _timeSinceStopped = 0;
            }
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

            _RunMan.ResetRunMan();
            _timeSinceStopped = 0;
        }
    }
}
