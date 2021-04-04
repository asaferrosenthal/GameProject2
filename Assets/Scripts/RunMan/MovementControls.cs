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

        [Tooltip("Stop player movement?")] 
        public bool _StopPlayer = false;
        
        // record of rigidbody to prevent need to re-access
        private Rigidbody _rigidbody;
        
        // Vector built by inputs for physics based movement
        private Vector3 _movement;

        public Transform _RotationBox;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            Cursor.visible = false;
        }
        
        private void FixedUpdate()
        {
            // transform save
            Transform trans = Camera.main.transform;

            // player rotation relative to camera look position
            transform.LookAt(_RotationBox);
            // correct any rotation in the x and z axis
            transform.rotation = new Quaternion(0f, transform.rotation.y, 0f, transform.rotation.w);
            
            // Inputs for each axis is defined in Input Manager
            // Fill out the _movement vector with given axis inputs

            _movement =  _ForwardSpeed * transform.forward;
            
            // Add force in the direction of rotation, make it feel less sloppy
            _movement += trans.right * (_HorizontalSpeed * Input.GetAxis("Horizontal"));
            
            // Apply movement vector to character using physics system
            if(!_StopPlayer) _rigidbody.AddForce(_movement, ForceMode.Impulse);
            
        }

    }
}
