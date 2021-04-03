using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace Traps
{
    public class GroundBreakTrap : Trap
    {
        [Tooltip("The time required before each floor tile begins to fall away")]
        [SerializeField] private float _Threshold = .25f;
        // Ground components
        private Rigidbody[] _rigidbodies;
        private Vector3[] _positions;
        private Quaternion[] _rotations;
        private Coroutine _groundBreaking;
        
        const int PlayerLayer = 10;
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
                _rigidbodies[i].isKinematic = true;
            }
        }

        protected override void ApplyTrap()
        {
            if (_Target.layer != PlayerLayer) return;
            
            if(_groundBreaking == null) _groundBreaking = StartCoroutine(GroundBreak());
        }
        
        private IEnumerator GroundBreak()
        {
            base.ApplyTrap();
            yield return new WaitForSeconds(_Threshold);

            // Turn off kinematic setting on rigidbodies in the wall
            foreach(Rigidbody ele in _rigidbodies)
            {
                ele.isKinematic = false;
            }
            _AudioSource.Play();
            _Enabled = false;
        }

        public override void ResetTrap()
        {
            if (_groundBreaking != null)
            {
                StopCoroutine(_groundBreaking);
                _groundBreaking = null;
            }
            
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
                
                // enable each brick
                ele.gameObject.SetActive(true);
                
                i++;
            }
        }
    }
}
