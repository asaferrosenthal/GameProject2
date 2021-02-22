using System;
using System.Collections.Generic;
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
        
        [Tooltip("How much the rotation vector will be scaled")]
        public float _RotationSpeed;

        [Tooltip("How far can the agent see around itself")]
        public float _SearchRadius = 5;
        
        [Tooltip("Toggles training specific functionality")]
        public bool _TrainingMode;
        
        [Tooltip("Toggles weather or not the agent should be changing")]
        public bool _Frozen;
        
        [Header("Required Information")]
        public int[] _TargetLayers;
        
        [Tooltip("What layers do we have negative consequences for interacting with")]
        public int[] _ObstacleLayers;

        [Tooltip("How much we reward the agent for achieving something, subject to static modifiers according to the situation")]
        public float _DefaultRewardValue;
        
        [Tooltip("How much we punish the agent for negative behaviour, subject to static modifiers according to the situation")]
        public float _DefaultPunishmentValue;
        
        // Agent Components and settings
        private Rigidbody _rigidBody;
        
        // world information
        private int _targetLayerMask;
        private int _obstacleLayerMask;
        
        // target information
        private List<GameObject> _targetRecords;
        private float _targetDistance;
        
        // Obstacle information
        private List<GameObject> _obstacleRecords;
        private float _obstacleDistance;
        
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
            _obstacleDistance = 0;
            _targetRecords = new List<GameObject>();
            _obstacleRecords = new List<GameObject>();
            _rigidBody.velocity = Vector3.zero;
            _rigidBody.angularVelocity = Vector3.zero;
        }
        private void UpdateAgentSenseData()
        {
            var position = transform.position;
            
            _targetRecords = CleanList.RemoveDead(_targetRecords, _targetLayerMask);
            _targetRecords = DetectColliderLayer.InLayerRadius(_targetLayerMask, _SearchRadius, position, _targetRecords);
            _targetRecords = Sort.ByDistance(_targetRecords, position);

            _obstacleRecords = CleanList.RemoveDead(_obstacleRecords, _obstacleLayerMask);
            _obstacleRecords = DetectColliderLayer.InLayerRadius(_obstacleLayerMask, _SearchRadius, position, _obstacleRecords);
            _obstacleRecords = Sort.ByDistance(_obstacleRecords, position);
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
            base.OnActionReceived(vectorAction);
        }

        public override void Heuristic(float[] actionsOut)
        {
            base.Heuristic(actionsOut);
        }
    }
}
