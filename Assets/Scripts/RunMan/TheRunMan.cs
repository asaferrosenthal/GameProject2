using System.Collections.Generic;
using Environment;
using UnityEngine;
using UnityEngine.Events;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace RunMan
{
    /// <summary>
    /// Scale Ability is responsible for the Run Man mechanic of the size increase/decrease mechanic that also affects mass
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class TheRunMan : MonoBehaviour
    {
        [Header("Run Man Settings")]
        [Tooltip("Factor by which inputs change Run Man scale")]
        public float _ScaleFactor = 0.2f;

        public bool _Training = false;
        public List<Transform> _TrainingLocations;
        
        // record of rigidbody to prevent need to re-access
        private Rigidbody _rigidBody;

        // value used for scaling Run Man object
        private float _currentScale = MinScale;

        // Use on awake scale value as the default Run Man scale_
        private Vector3 _defaultScale;
        
        // Use on awake mass value as default
        private float _defaultMass;
        
        // live mass for updates
        private float _currentMass;
        
        // Use on reset
        private Vector3 _defaultPosition;
        
        // use on reset
        private Quaternion _defaultRotation;

        // Set maximum and minimum scaling of run man, could make these public for testing
        private const float MaxScale = 1.5f;
        private const float MinScale = 1f;

        // Initializes defaults from the rigidbody attached to a shared game object
        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody>();
            Transform trans = transform;
            _defaultScale = trans.localScale;
            var mass = _rigidBody.mass;
            _defaultMass = mass;
            _currentMass = mass;
            _defaultRotation = trans.localRotation;
            _defaultPosition = trans.localPosition;
        }

        private void Update()
        {
            UpdateRunManScale();
            ScaleRunMan();
            // Assign end of game trigger event
            
        }

        /// <summary>
        /// Uses exclusively mousewheel input to scale Run Man up and down, could modify Scroll input settings for additional inputs
        /// </summary>
        private void UpdateRunManScale()
        {
            _currentScale = Mathf.Clamp(_currentScale + -1* (Input.GetAxis("Scroll") * _ScaleFactor), MinScale, MaxScale);
        }

        /// <summary>
        /// Apply the current scale to the actual game object of Run Man
        /// </summary>
        private void ScaleRunMan()
        {
            transform.localScale = _defaultScale * _currentScale;
            _rigidBody.mass = _currentMass * _currentScale;
        }

        public void ResetRunMan()
        {
            Transform trans = transform;
            trans.localScale = _defaultScale;
            _currentScale = MinScale;
            _rigidBody.mass = _defaultMass;
            _currentMass = _defaultMass;
            _rigidBody.velocity = Vector3.zero;
            _rigidBody.angularVelocity = Vector3.zero;
            trans.localRotation = _defaultRotation;
            if (_Training)
            {
                trans.position = RunManTrainingAssistant.RandomizeSpawn(_TrainingLocations, gameObject);
            }
            else
            {
                trans.localPosition = _defaultPosition;
            }
            
            
        }

        public void AddMass(float num)
        {
            _currentMass += num;
            _rigidBody.mass += num;
        }
        
    }
}