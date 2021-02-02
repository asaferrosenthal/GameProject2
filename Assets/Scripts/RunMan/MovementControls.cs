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

        // to feel better
        private float _rotationSpeed;
        
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

        // Keeps track of total rotation
        private float _totalRotation = 0f;

        // Set max values for rotation
        private float _maxRotation = 30f;
        private float _minRotation = -30f;
 
        
        // Initializes default settings for movement functionality
        // The separation of default and actual speeds is to allow for external runtime speed modification without additional storage of the original speed, could remove for testing purposes
        private void Awake()
        {
            _speed = _DefaultSpeed;
            _jumpHeight = _DefaultJumpHeight;
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            // Inputs for each axis is defined in Input Manager

            // Fill out the _movement vector with given axis inputs
            //_movement = Input.GetAxis("Vertical") * _speed * transform.forward;

            // Apply movement vector to character using physics system
            _rigidbody.AddForce(Vector3.forward * _speed);

            // Apply rotation to character
            rotate();
        }

        // Rotates character based on input
        private void rotate()
        {
            // Fill out the _rotation vector with given axis inputs
            _rotation.y = Input.GetAxis("Horizontal") * _DefaultRotationSpeed;
            _totalRotation += _rotation.y;

            // Apply rotation vector to character
            if (_totalRotation >= _minRotation && _totalRotation <= _maxRotation)
            {
                transform.Rotate(_rotation * setRotationSpeed());
            }

            _totalRotation = Mathf.Clamp(_totalRotation, _minRotation, _maxRotation);

        }

        // To feel better
        private float setRotationSpeed()
        {
            if (Mathf.Abs(_totalRotation) <= (_maxRotation/4))
            {
                _rotationSpeed = _DefaultRotationSpeed * 2f;
            }

            else if (Mathf.Abs(_totalRotation) >= (_maxRotation*(3/4)))
            {
                _rotationSpeed = _DefaultRotationSpeed / 2f;
            }
            
            else
            {
                _rotationSpeed = _DefaultRotationSpeed;
            }

            return _rotationSpeed;
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
