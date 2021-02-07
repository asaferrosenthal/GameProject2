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

        // Keeps track of total rotation
        private float _totalRotation;

        // Sets speed of rotation
        private float _rotationSpeed;

        // Keeps track of current rotation
        private float _currentRotation;

        // Set max values for rotation
        private float _maxRotation;
        private float _minRotation;

        // Initializes default settings for movement functionality
        // The separation of default and actual speeds is to allow for external runtime speed modification without additional storage of the original speed, could remove for testing purposes
        private void Awake()
        {
            _speed = _DefaultSpeed;
            _rigidbody = GetComponent<Rigidbody>();
            setInitRotation(30f);
            _jumpHeight = _DefaultJumpHeight;
        }

        // Sets initial rotation values
        private void setInitRotation(float maxRotationDistance)
        {
            _currentRotation = 0f;
            _totalRotation = 0f;
            _maxRotation = maxRotationDistance;
            _minRotation = -maxRotationDistance;           
        }

        private void FixedUpdate()
        {
            // Inputs for each axis is defined in Input Manager

            // Apply movement vector to character using physics system
            _rigidbody.velocity = autoMove();

            // Debug.Log("\nspeed: " + _rigidbody.velocity + "\n" + "mass: " + _rigidbody.mass);

            // Apply rotation to character
            rotate();
        }

        // Gets velocity for auto movement speed
        private Vector3 autoMove()
        {
            Vector3 movementVelocity;
            float _defaultMass = GetDefaultMass();
            float horizontalSpeed = setHorizontalSpeed();
            float verticalSpeed = 1 - Mathf.Abs(horizontalSpeed);
            movementVelocity = new Vector3(horizontalSpeed, 0, verticalSpeed);
            movementVelocity *= scaledSpeed();
            return movementVelocity;
        }

        // Sets the horizontal speed
        private float setHorizontalSpeed()
        {
            float horizontalSpeed;
            horizontalSpeed = _totalRotation / 100;
            return horizontalSpeed;
        }

        // Calculates speed of run man, based on its scale
        private float scaledSpeed()
        {
            float scaledSpeed;
            float _defaultMass = GetDefaultMass();
            scaledSpeed = _speed + (_DefaultSpeed * (_defaultMass - _rigidbody.mass)) ;
            return scaledSpeed;
        }

        // Gets the default mass of the run man
        private float GetDefaultMass()
        {
            float _defaultMass;
            _defaultMass = _rigidbody.gameObject.GetComponent<ScaleAbility>().GetDefaultMass();
            return _defaultMass;
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
                _rotationSpeed = setRotationSpeed();
                //Debug.Log("rotation speed" + _rotationSpeed);
                transform.Rotate(_rotation * _rotationSpeed);
            }

            _totalRotation = Mathf.Clamp(_totalRotation, _minRotation, _maxRotation);
        }

        // Sets the rotation speed
        private float setRotationSpeed()
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            if (horizontalInput != 0) //rotating
            {
                // slow down at edges of rotation
                if (Mathf.Abs(_totalRotation) >= (_maxRotation * (3f / 4f)))
                {
                    // check rotation direction directed towards edge
                    if ((_totalRotation < 0 && horizontalInput < 0) ||
                        (_totalRotation > 0 && horizontalInput > 0))
                    {
                        _rotationSpeed = _DefaultRotationSpeed / 8f;
                    }
                }
                // speed up during rotation
                else
                {
                    _currentRotation += horizontalInput;
                    _rotationSpeed = 1f + (Mathf.Abs(_currentRotation) / (_maxRotation * 1.75f));
                }

            }
            else // not rotating
            {
                _currentRotation = 0f;
            }
            return _rotationSpeed;
        }

        // Collision detection for jumping mechanic (in the event we decide to add this to the project)
        private void OnCollisionStay(Collision other)
        {
            //Debug.Log("Colliding with something");
            // Is the layer of the collision also a floor layer?
            if (1 << other.gameObject.layer != Floor) return; // if not then leave
            
            //Debug.Log("Jumping");
            // Eventually a check to see if we can jump
            // Jumping vector from given axis inputs
            _jump = Input.GetAxis("Jump") * _jumpHeight * transform.up;
                
            // Apply jump vector
            _rigidbody.AddForce(_jump);
        }
    }
}
