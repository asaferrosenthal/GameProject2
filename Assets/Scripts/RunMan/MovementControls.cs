using System;
using UnityEngine;
using static UnityEngine.Vector3;

namespace RunMan
{
    [RequireComponent(typeof(Rigidbody))]
    public class MovementControls : MonoBehaviour
    {
        private const int Floor = 1 << 8;
        private const int Wall = 1 << 9;
        
        [Header("Movement Settings")]
        
        [Tooltip("Factor movement inputs will be scaled by")]
        public float _ForwardSpeed = 1f;

        [Tooltip("Factor by which horizontal movement will be scaled by")]
        public float _HorizontalSpeed = 1f;

        [Tooltip("Factor rotation inputs will be scaled by")]
        public float _DefaultRotationSpeed;
        
        [Tooltip("Does the player rotate with the mouse position or the keyboard inputs.")]
        public bool _MouseRotation = true;
        
        // record of rigidbody to prevent need to re-access
        private Rigidbody _rigidbody;
        
        // Vector built by inputs for physics based movement
        private Vector3 _movement;

        // Vector built by inputs for rotation translation
        private Vector3 _rotation;
        
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            Cursor.visible = false;
        }
        
        private void FixedUpdate()
        {
            
            // transform save
            Transform trans = transform;
            
            // Inputs for each axis is defined in Input Manager
            // Fill out the _movement vector with given axis inputs

            _movement =  _ForwardSpeed * trans.forward;
            
            // Add force in the direction of rotation, make it feel less sloppy
            _movement += trans.right * (_HorizontalSpeed * Input.GetAxis("Horizontal"));

            if (_MouseRotation & ( (Input.GetMouseButton(0) | Input.GetMouseButton(1)) ))
            {
                transform.eulerAngles += up * (Input.GetAxis("Mouse X") * _DefaultRotationSpeed);
            }
            else
            {
                // Fill out the _rotation vector with given axis inputs
                _rotation.y = Input.GetAxis("Horizontal") * _DefaultRotationSpeed;
                // Apply rotation vector to character
                transform.Rotate(_rotation);
            }

            // Apply movement vector to character using physics system
            _rigidbody.AddForce(_movement);
        }

    }
}
