using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Utility;
using static Environment.RunManTrainingAssistant;
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
        
        [Tooltip("The Animator responsible for the logic of the player animation")]
        public PlayerAnimationController _AnimationController;

        [Tooltip("The run man camera controller")]
        public PlayerCamera _PlayerCamera;

        [Tooltip("Run man movement control reference")]
        public MovementControls _Movement;

        [Tooltip("List of goober visualizers")]
        public List<GameObject> _GooberVisualizer;
        
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
        
        // use on reset
        private int _defaultLayer;

        // how many goobers have hit the player
        private int _hits = 0;

        // Set maximum and minimum scaling of run man, could make these public for testing
        private const float MaxScale = 1.5f;
        private const float MinScale = 1f;

        // Set colors for mass change
        public Color _colorBig = Color.red;
        
        private Color _originalColour;
        float duration = 1.0f;
        private SkinnedMeshRenderer _mSkinnedMeshRenderer;


        // Initializes defaults from the rigidbody attached to a shared game object
        private void Awake()
        {
            _defaultLayer = gameObject.layer;
            _rigidBody = GetComponent<Rigidbody>();
            Transform trans = transform;
            _defaultScale = trans.localScale;
            var mass = _rigidBody.mass;
            _defaultMass = mass;
            _currentMass = mass;
            _defaultRotation = trans.rotation;
            _defaultPosition = trans.position;
            
            // ensure visualizers are off
            foreach (GameObject ele in _GooberVisualizer)
            {
                ele.SetActive(false);
            }
        }

        private void Start()
        {
            ResetRunMan();
            //get skin renderer for run man's dummy 
            _mSkinnedMeshRenderer = GameObject.Find("Dummy").GetComponent<SkinnedMeshRenderer>();
            _originalColour = _mSkinnedMeshRenderer.material.color;
        
        }

        private void Update()
        {
            UpdateRunManScale();
            ScaleRunMan();
        }

        /// <summary>
        /// Uses exclusively mousewheel input to scale Run Man up and down, could modify Scroll input settings for additional inputs
        /// </summary>
        private void UpdateRunManScale()
        {
            // clamp the scale of the character between max and min
            _currentScale = Mathf.Clamp(_currentScale + -1* (Input.GetAxis("Scroll") * _ScaleFactor), MinScale, MaxScale);
            
            //mechanics for runman's colour change according to mass
            _mSkinnedMeshRenderer.material.color = Color.Lerp(_originalColour, _colorBig, _currentScale/MaxScale);
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
            // for multi-reference
            Transform trans = transform;
            
            // set default and current scale respectively
            trans.localScale = _defaultScale;
            _currentScale = MinScale;
            
            // set default and current mass respectively
            _rigidBody.mass = _defaultMass;
            _currentMass = _defaultMass;
            
            // reset the velocities of the Run Man
            _rigidBody.velocity = Vector3.zero;
            _rigidBody.angularVelocity = Vector3.zero;
            
            // reset the rotation of the run man
            trans.rotation = _defaultRotation;
            
            // reset the object layer
            gameObject.layer = _defaultLayer;

            // reset object position
            trans.position = _defaultPosition;
            
            // reset hits on player
            _hits = 0;
            foreach (GameObject ele in _GooberVisualizer)
            {
                ele.SetActive(false);
            }

            // reset player movement
            _Movement._StopPlayer = false;
            
            // reset the player animation
            _AnimationController.GoIdle();
            
            // reset the camera relative to run man
            _PlayerCamera.Reset();

        }

        public void AddMass(float num)
        {
            _currentMass += num;
            _rigidBody.mass += num;
            if (_GooberVisualizer.Count <= _hits) return;
            _GooberVisualizer[_hits].SetActive(true);
            _hits++;
        }

        /// <summary>
        /// Multiples the player speed by the input value
        /// </summary>
        /// <param name="num"></param>
        public void AlterVelocity(float num)
        {
            _rigidBody.velocity *= num;
        }
    }
}