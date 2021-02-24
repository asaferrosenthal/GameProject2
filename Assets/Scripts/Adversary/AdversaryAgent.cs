using System;
using System.Collections.Generic;
using Traps;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine;
using Utility;

namespace Adversary
{
    public class AdversaryAgent : Agent
    {
        [Header("Settings")]
        [Tooltip("How much the movement vector will be scaled")]
        public float _MoveForce;
        
        [Tooltip("How much the jump vector will be scaled")]
        public float _JumpForce;
        
        [Tooltip("How much the Y rotation vector will be scaled")]
        public float _YawRotationSpeed;

        [Tooltip("How far can the agent see around itself")]
        public float _SearchRadius = 5;
        
        [Tooltip("Toggles training specific functionality")]
        public bool _TrainingMode;
        
        [Tooltip("Toggles weather or not the agent should be changing")]
        public bool _Frozen;
        
        [Header("Required Information")]
        [Tooltip("What layers do we have positive consequences for interacting with")]
        public int[] _TargetLayers;
        
        [Tooltip("What layers do we have negative consequences for interacting with")]
        public int[] _ObstacleLayers;

        [Tooltip("How much we reward the agent for achieving something, subject to static modifiers according to the situation")]
        public float _DefaultRewardValue;
        
        [Tooltip("How much we punish the agent for negative behaviour, subject to static modifiers according to the situation")]
        public float _DefaultPunishmentValue;
        
        // Agent Components and settings
        private Rigidbody _rigidBody;
        private float _smoothYawChange = 0f;
        
        // world information
        private int _targetLayerMask;
        private int _obstacleLayerMask; 

        /* Test Area*/
        
        // Nearest of each trap type
        private SpeedTrap _accelerator;

        private SpeedTrap _deccelerator;

        private ExplosionTrap _explosionTrap;

        private DestructableWall _destructableWall;
        
        /*Test Area*/
        
        // target information
        private List<GameObject> _targetRecords;
        private float _targetDistance;
        
        /*// Obstacle information
        private List<GameObject> _obstacleRecords;
        private float _obstacleDistance;*/
        
        // Initialize information that will not change
        private void Awake()
        {
            // Create the full layer masks for targets and obstacles
            foreach (var layer in _TargetLayers)
            {
                _targetLayerMask = _targetLayerMask | (1 << layer);
            }

            foreach (var layer in _ObstacleLayers)
            {
                _obstacleLayerMask = _obstacleLayerMask | (1 << layer);
            }

            // Rigid body for physics interactions
            _rigidBody = GetComponentInChildren<Rigidbody>();
        }

        private void Start()
        {
            ResetAgent();
        }

        private void ResetAgent()
        {
            // reset all physics and memory
            _targetDistance = 0;
            _targetRecords = new List<GameObject>();
            _rigidBody.velocity = Vector3.zero;
            _rigidBody.angularVelocity = Vector3.zero;
        }
        private void UpdateAgentSenseData()
        {
            var position = transform.position;
            
            _targetRecords = CleanList.RemoveDead(_targetRecords, _targetLayerMask);
            _targetRecords = DetectColliderLayer.InLayerRadius(_targetLayerMask, _SearchRadius, position, _targetRecords);
            _targetRecords = Sort.ByDistance(_targetRecords, position);
            
        }

        /*ML Agent specific methods*/
        public override void OnEpisodeBegin()
        {
            base.OnEpisodeBegin();
        }

        public override void CollectObservations(VectorSensor sensor)
        {
            base.CollectObservations(sensor);
        }

        public override void OnActionReceived(float[] vectorAction)
        {
            // if frozen don't take any actions
            if (_Frozen)
            {
                _rigidBody.velocity = Vector3.zero;
                _rigidBody.angularVelocity = Vector3.zero;
                return;
            }
            
            // Calculate force applied
            Vector3 move = (Vector3.forward * _MoveForce * vectorAction[0]) + (Vector3.up * _JumpForce * vectorAction[1]);

            // Apply move force
            _rigidBody.AddForce(move * _MoveForce);
            
            // Calculate rotation applied
            Vector3 rotationVector = transform.rotation.eulerAngles;
            
            // Calculate yaw 
            float yawChange = vectorAction[3];
            
            // Smooth yaw rotation
            _smoothYawChange = Mathf.MoveTowards(_smoothYawChange, yawChange, 2f * Time.fixedDeltaTime);
            float yaw = rotationVector.y + _smoothYawChange * Time.fixedDeltaTime * _YawRotationSpeed;
            
            // Apply rotation vector
            transform.rotation = Quaternion.Euler(0f,yaw,0f);
            
            // Re-evaluate data
            UpdateAgentSenseData();
        }

    }
}
