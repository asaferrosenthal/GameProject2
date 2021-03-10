using System.Collections.Generic;
using Environment;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine;
using Utility;
using static UnityEngine.Vector3;

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

        public EnvironmentManager _Manager;
        public Spawner _Spawner;
        
        public bool _Master;
        // Agent Components and settings
        private Rigidbody _rigidBody;
        private InteractionBase _goober;
        private bool _isGrounded = false;
        private float _smoothYawChange = 0f;
        
        // world information
        private int _targetLayerMask;
        private int _obstacleLayerMask;
        //private int _trapLayerMask;
        
        // target information
        private List<GameObject> _targetRecords = new List<GameObject>();
        
        // Obstacle information
        private List<GameObject> _obstacleRecords = new List<GameObject>();
        
        // used in training to track how many time it successfully hit a reward per episode
        private float _hits;

        // Constants
        private const int FloorLayer = 1 << 8;

        // Initialize information that will not change
        private void Awake()
        {
            // Create the full layer masks for targets and obstacles
            foreach (var layer in _TargetLayers)
            {
                _targetLayerMask |= (1 << layer);
            }

            foreach (var layer in _ObstacleLayers)
            {
                _obstacleLayerMask |= (1 << layer);
            }
            
            // Rigid body for physics interactions
            _rigidBody = GetComponentInChildren<Rigidbody>();
            _goober = GetComponent<InteractionBase>();
            _Manager = transform.parent.transform.parent.GetComponentInParent<EnvironmentManager>();

        }

        private void Start()
        {
            _Manager._Training = _TrainingMode;
        }
        
        public override void OnEpisodeBegin()
        {
            if (!_TrainingMode) return;
            
            if (_Master) _Manager.ResetEnvironment();
            ResetAgent();
        }

        public override void CollectObservations(VectorSensor sensor)
        {
            // This agents information

            // Observe the agent's rotation : 3
            sensor.AddObservation(_rigidBody.rotation.normalized);
            // Are we currently on the ground
            sensor.AddObservation(_isGrounded); // 1

            // Obstacle related information
            if (_obstacleRecords.Count <= 0)
            {
                sensor.AddObservation(new float[3]);
            }
            else // records obstacle information if they exist
            {
                // obstacle information
                Vector3 transformPosition = transform.position;
                Vector3 dirOfObstacle = (_obstacleRecords[0].transform.position - transformPosition).normalized; 
                float obstacleDistance = (_obstacleRecords[0].transform.position - transformPosition).magnitude;
                
                // relative distance to obstacle
                sensor.AddObservation(obstacleDistance/_SearchRadius); // 1
                // Where is the obstacle relative to agent ( -1 means behind, left of, beneath. 1 means in front, right of, above)
                sensor.AddObservation(Dot(dirOfObstacle, -_obstacleRecords[0].transform.forward.normalized)); // 1
                sensor.AddObservation(Dot(dirOfObstacle, -_obstacleRecords[0].transform.right.normalized)); // 1
            }
            
            // Targets related information
            if (_targetRecords.Count <= 0)
            {
                sensor.AddObservation(new float[3]);
            }
            else
            {
                Vector3 agentPosition = transform.position;
                Vector3 targetPosition = _targetRecords[0].transform.position;
                Vector3 dirOfTarget = (targetPosition - agentPosition).normalized;
                float targetDistance = (targetPosition - agentPosition).magnitude;
                
                // relative distance to target
                sensor.AddObservation(targetDistance/_SearchRadius); // 1
                // Where is the target relative to the agent ( -1 means behind, left of, beneath. 1 means in front, right of, above)
                sensor.AddObservation(Dot(dirOfTarget, -_targetRecords[0].transform.forward.normalized)); // 1
                sensor.AddObservation(Dot(dirOfTarget, -_targetRecords[0].transform.right.normalized)); // 1
                
            }

        }

        public override void OnActionReceived(float[] vectorAction)
        {
            // if frozen don't take any actions
            if (_Frozen)
            {
                _rigidBody.velocity = zero;
                _rigidBody.angularVelocity = zero;
                return;
            }

            // are we on the ground, and are we moving
            if (_isGrounded)
            {
                Vector3 move = new Vector3();
                // Z axis
                move += forward * vectorAction[0] * _MoveForce;
                // X axis
                move += right * vectorAction[1] * _MoveForce;
                // Y axis
                move += up * _JumpForce;

                // Apply move force
                _rigidBody.AddForce(move);
            }
            
            // Calculate rotation applied
            _rigidBody.transform.Rotate(transform.rotation.eulerAngles,vectorAction[2] * _YawRotationSpeed);
            
            // Re-evaluate data
            UpdateAgentSenseData();
        }
        
        // Bundled stay and enter together due to desired behaviour is the same in both cases
        private void OnTriggerStayOrEnter(Collider other)
        {
            float reward = 0;

            if ((_targetLayerMask | (1 << other.gameObject.layer)) == _targetLayerMask) // positive interaction
            {
                float bonus = 0;
                
                if(_TrainingMode) other.gameObject.layer = 7; // the dead layer
                _targetRecords.Remove(other.gameObject);

                // 10% bonus for every target hit
                bonus = _hits * .1f;
                reward = bonus + _DefaultRewardValue;
            }
            else if ((_obstacleLayerMask | (1 << other.gameObject.layer)) == _obstacleLayerMask) // negative interaction
            {
                reward = _DefaultPunishmentValue;
            }
            
            // Re-evaluate data
            UpdateAgentSenseData();
            
            // adds no reward if neither interaction happened
            AddReward(reward);
        }

        private void OnTriggerStay(Collider other)
        {
            OnTriggerStayOrEnter(other);
        }

        private void OnTriggerEnter(Collider other)
        {
            OnTriggerStayOrEnter(other);
        }
        
        private void OnCollisionEnter(Collision other)
        {
            if (1 << other.gameObject.layer == FloorLayer) _isGrounded = true;
        }

        private void OnCollisionExit(Collision other)
        {
            if (1 << other.gameObject.layer == FloorLayer) _isGrounded = false;
        }

        private void UpdateAgentSenseData()
        {
            if (_Frozen) return;
            
            var position = transform.position;
            
            // Update target data
            _targetRecords = CleanList.RemoveDead(_targetRecords, _targetLayerMask);
            _targetRecords = DetectColliderLayer.InLayerRadius(_targetLayerMask, _SearchRadius, position, _targetRecords);
            _targetRecords = Sort.ByDistance(_obstacleRecords, position);

            // Update obstacle data
            _obstacleRecords = CleanList.RemoveDead(_obstacleRecords, _obstacleLayerMask);
            _obstacleRecords = DetectColliderLayer.InLayerRadius(_obstacleLayerMask, _SearchRadius, position, _obstacleRecords);
            _obstacleRecords = Sort.ByDistance(_obstacleRecords, position);

        }
        
        public void ResetAgent()
        {
            // reset all physics and memory
            _targetRecords = new List<GameObject>();
            _obstacleRecords = new List<GameObject>();
            _rigidBody.velocity = zero;
            _rigidBody.angularVelocity = zero;
            _Frozen = false;
            if(_goober != null) _goober.Reset();
            _Spawner.RespawnAgent(this);
            _hits = 0;
            UpdateAgentSenseData();
        }

        
    }
}
