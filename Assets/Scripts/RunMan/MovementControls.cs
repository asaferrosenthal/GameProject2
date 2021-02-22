using System;
using UnityEngine;

namespace RunMan
{
    [RequireComponent(typeof(Rigidbody))]
    public class MovementControls : MonoBehaviour
    {
        private const int Floor = 1<<8;
        private const int Wall = 1 << 9;
        
        [Header("Movement Settings")]
        
        [Tooltip("Factor movement inputs will be scaled by")]
        public float _DefaultSpeed = 1f;
        
        [Tooltip("Factor jump inputs will be scaled by")]
        public float _DefaultJumpHeight = 1f;

        [Tooltip("Factor rotation inputs will be scaled by")]
        public float _DefaultRotationSpeed;

        // record of rigidbody to prevent need to re-access
        private Rigidbody _rigidbody;
        
        // Vector built by inputs for physics based movement
        private Vector3 _movement;
        
        // Vector built by inputs for physics based jumping
        private Vector3 _jump;
        
        // Vector built by inputs for rotation translation
        private Vector3 _rotation;

        // used for turning speed back to what it was on spawn
        private float _speed;
        
        private void Awake()
        {
            _speed = _DefaultSpeed;
            _rigidbody = GetComponent<Rigidbody>();
        }
        private void FixedUpdate()
        {
            // Inputs for each axis is defined in Input Manager
            
            // Fill out the _movement vector with given axis inputs
            _movement =  _DefaultSpeed * transform.forward;
            // Add force in the direction of rotation, make it feel less sloppy
            _movement += transform.right * (_DefaultSpeed * Input.GetAxis("Horizontal"));
            
            // Fill out the _rotation vector with given axis inputs
            _rotation.y = Input.GetAxis("Horizontal") * _DefaultRotationSpeed;
            
            // Apply movement vector to character using physics system
            _rigidbody.AddForce(_movement);
            
            // Apply rotation vector to character
            transform.Rotate(_rotation);

        }

        private void OnCollisionStay(Collision other)
        {
            if (1 << other.gameObject.layer != Floor) return;
            // Jumping vector from given axis inputs
            _jump = Input.GetAxis("Jump") * _DefaultJumpHeight * transform.up;
                
            // Apply jump vector
            _rigidbody.AddForce(_jump);
        }
        
    }
}
