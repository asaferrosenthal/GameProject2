using System;
using UnityEngine;

//Need to include to delay code execution
using System.Threading;
using System.Threading.Tasks;

namespace Traps {
    public class BreakGround : Trap
    {
        [Tooltip("The amount of time that the ground takes to break")]
        public int _BreakTime;
        
        private Rigidbody[] _rigidbodies;
        private Vector3[] _positions;
        private Quaternion[] _rotations;

        private void Awake()
        {
            // Initialize each array for wall components
            _rigidbodies = GetComponentsInChildren<Rigidbody>();
            _positions = new Vector3[_rigidbodies.Length];
            _rotations = new Quaternion[_rigidbodies.Length];
            
            // Save initial state
            for (int i = 0; i < _rigidbodies.Length; i++)
            {
                _positions[i] = _rigidbodies[i].transform.position;
                _rotations[i] = _rigidbodies[i].transform.rotation;
            }

            //turn on kinematic setting 
            foreach (Rigidbody ele in _rigidbodies)
            {
                ele.isKinematic = true;
            }
        }

        protected async override void ApplyTrap()
        {
            base.ApplyTrap();
            Rigidbody tar = _Target.GetComponent<Rigidbody>();
            if (tar == null) return;

            //delay for 10 seconds after runman leaves
            await Task.Delay(_BreakTime * 1000);

            // Turn off kinematic setting of rigidbodies on the ground, makes the ground "break"
            foreach (Rigidbody ele in _rigidbodies)
            {
                ele.isKinematic = false;
            }
            
        }

        
        public override void ResetTrap()
        {
            base.ResetTrap();
            
            int i = 0;
            foreach (Rigidbody ele in _rigidbodies)
            {
                // Stop the Rigidbodies from moving
                ele.isKinematic = true;
                var transform1 = ele.transform;
                
                // Reset positions
                transform1.position = _positions[i];
                transform1.rotation = _rotations[i];
                i++;
            }
            
        }
        
    }
}

