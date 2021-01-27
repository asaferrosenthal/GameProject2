using UnityEngine;

namespace RunMan
{
    /// <summary>
    /// Movement Controls is a physics based movement script designed for player input
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class MovementControls : MonoBehaviour
    {
        private const int Floor = 1<<8;
        [Header("Movement Settings")]
        
        [Tooltip("Factor movement inputs will be scaled by")]
        public float _DefaultSpeed = 1f;
        
        [Tooltip("Factor jump inputs will be scaled by")]
        public float _DefaultJumpHeight = 1f;

        [Tooltip("Factor rotation inputs will be scaled by")]
        public float _DefaultRotationSpeed;
        // Updatable speed value to be modified by interactions
        private float _speed;
        
        // Updatable jump value to be modified by interactions
        private float _jumpHeight;
        
        // record of rigidbody to prevent need to re-access
        private Rigidbody _rigidbody;
        
        // Vector built by inputs for physics based movement
        private Vector3 _movement;
        
        // Vector built by inputs for physics based jumping
        private Vector3 _jump;
        
        // Vector built by inputs for rotation translation
        private Vector3 _rotation;
        
        // Initializes default settings for movement functionality
        // The separation of default and actual speeds is to allow for external runtime speed modification without additional storage of the original speed, could remove for testing purposes
        private void Awake()
        {
            _speed = _DefaultSpeed;
            _jumpHeight = _DefaultJumpHeight;
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody = GetComponent<Rigidbody>();
        }
        private void FixedUpdate()
        {
            // Inputs for each axis is defined in Input Manager
            
            // Fill out the _movement vector with given axis inputs
            _movement = Input.GetAxis("Vertical") * _speed * transform.forward;

            // Fill out the _rotation vector with given axis inputs
            _rotation.y = Input.GetAxis("Horizontal") * _DefaultRotationSpeed;
            
            // Apply movement vector to character using physics system
            _rigidbody.AddForce(_movement);

            // Apply rotation vector to character
            transform.Rotate(_rotation);
        }
        
        // Collision detection for jumping mechanic (in the event we decide to add this to the project)
        private void OnCollisionStay(Collision other)
        {
            Debug.Log("Colliding with something");
            // Is the layer of the collision also a floor layer?
            if (1 << other.gameObject.layer != Floor) return; // if not then leave
            
            Debug.Log("Jumping");
            // Eventually a check to see if we can jump
            // Jumping vector from given axis inputs
            _jump = Input.GetAxis("Jump") * _jumpHeight * transform.up;
                
            // Apply jump vector
            _rigidbody.AddForce(_jump);
        }
    }
}
