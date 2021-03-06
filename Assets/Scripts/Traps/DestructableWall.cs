using System;
using UnityEngine;
using Utility;

namespace Traps
{
    public class DestructableWall : Trap
    {
        [Tooltip("The momentum required to break through this wall")]
        public float _BreakThreshold;

        public AudioClip _BreakAudio, _BounceAudio;
        
        [SerializeField] private float _PushBack = -25;
        
        // Wall components
        private Rigidbody[] _rigidbodies;
        private Vector3[] _positions;
        private Quaternion[] _rotations;
        private BoxCollider _thisCollider;
        
        private void Awake()
        {
            // Initialize each array for wall components
            _rigidbodies = GetComponentsInChildren<Rigidbody>();
            _positions = new Vector3[_rigidbodies.Length];
            _rotations = new Quaternion[_rigidbodies.Length];
            _thisCollider = GetComponent<BoxCollider>();
            
            // Save initial state
            for (int i = 0; i < _rigidbodies.Length; i++)
            {
                _positions[i] = _rigidbodies[i].transform.position;
                _rotations[i] = _rigidbodies[i].transform.rotation;
            }
        }

        protected override void ApplyTrap()
        {
            // only react to the player hitting it
            if (_Target.layer != 10) return;
            
            Rigidbody tar = _Target.GetComponentInParent<Rigidbody>();
            
            if (tar == null) return; // there is no rigidbody

            if ((MomentumChecker.GetMomentum(tar) <= _BreakThreshold))
            {
                tar.AddForce(Vector3.forward * _PushBack, ForceMode.Impulse);
                _AudioSource.clip = _BounceAudio;
                _AudioSource.Play();
                return; // the threshold is not met
            }
            
            _Enabled = false;
            
            // disable trigger for wall
            _thisCollider.enabled = false;
            
            // Turn off kinematic setting on rigidbodies in the wall
            foreach (Rigidbody ele in _rigidbodies)
            {
                ele.isKinematic = false;
            }

            _AudioSource.clip = _BreakAudio;
            _AudioSource.Play();

        }

        public override void ResetTrap()
        {
            base.ResetTrap();

            // enable the trigger for wall
            _thisCollider.enabled = true;
            
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
