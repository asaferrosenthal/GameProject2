using System;
using System.Collections.Generic;
using System.Linq;
using Environment;
using Traps;
using Unity.Barracuda;
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
        private Rigidbody _target;
        
        // Obstacle information
        private List<GameObject> _obstacleRecords = new List<GameObject>();

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
            Debug.Log(_targetLayerMask);
            Debug.Log(_obstacleLayerMask);
        }

        private void Start()
        {
            _Manager._Training = _TrainingMode;
        }
        
        // Bundled stay and enter together due to desired behaviour is the same in both cases
        private void OnTriggerStayOrEnter(Collider other)
        {
            float reward = 0;

            if ((_targetLayerMask | (1 << other.gameObject.layer)) == _targetLayerMask) // positive interaction
            {
                float bonusMultiplier = _target.mass; // the heavier the target is, the greater the reward
                reward = bonusMultiplier * _DefaultRewardValue;
            }
            else if ((_obstacleLayerMask | (1 << other.gameObject.layer)) == _obstacleLayerMask) // negative interaction
            {
                reward = _DefaultPunishmentValue;
            }
            
            UpdateAgentSenseData();
            // adds no reward if neither interaction happened
            Debug.Log("Reward Given: " + reward);
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

        private void OnCollisionStay(Collision other)
        {
            
        }

        private void OnCollisionEnter(Collision other)
        {
            if (1 << other.gameObject.layer == FloorLayer) _isGrounded = true;
            UpdateAgentSenseData();
        }

        private void OnCollisionExit(Collision other)
        {
            if (1 << other.gameObject.layer == FloorLayer) _isGrounded = false;
            UpdateAgentSenseData();
        }

        private void UpdateAgentSenseData()
        {
            var position = transform.position;
            
            // Update target data
            // Get targets on possible layers
            Collider[] potentialTargets = Physics.OverlapSphere(position, _SearchRadius, _targetLayerMask);
            
            // is there any visible target? if so get the closest, otherwise set target to null
            if (potentialTargets.Length != 0) _target = potentialTargets[0].attachedRigidbody;

            // Update obstacle data
            _obstacleRecords = CleanList.RemoveDead(_obstacleRecords, _obstacleLayerMask);
            _obstacleRecords = DetectColliderLayer.InLayerRadius(_obstacleLayerMask, _SearchRadius, position, _obstacleRecords);
            _obstacleRecords = Sort.ByDistance(_obstacleRecords, position);

        }
        
        public void ResetAgent()
        {
            // reset all physics and memory
            _target = null;
            _obstacleRecords = new List<GameObject>();
            _rigidBody.velocity = zero;
            _rigidBody.angularVelocity = zero;
            _Frozen = false;
            _goober.Reset();
            _Spawner.RespawnAgent(this);
            UpdateAgentSenseData();
        }

        /*ML Agent specific methods*/
        public override void OnEpisodeBegin()
        {
            if (_Master) _Manager.ResetEnvironment();
            ResetAgent();
        }

        public override void CollectObservations(VectorSensor sensor)
        {    
            // Total of 35 points
            // This agents information
            // The current direction the agent is heading
            sensor.AddObservation(_rigidBody.velocity.normalized); // 3
            // The current velocity of the agent
            sensor.AddObservation(_rigidBody.velocity.magnitude); // 1
            // The current direction of the agent's angular velocity
            sensor.AddObservation(_rigidBody.angularVelocity.normalized); // 3
            // The current angular velocity of the agent
            sensor.AddObservation(_rigidBody.angularVelocity.magnitude); // 1
            // The current direction we are facing
            sensor.AddObservation(_rigidBody.rotation.normalized); // 3
            // Are we currently on the ground
            sensor.AddObservation(_isGrounded); // 1
            
            // Are we frozen?
            sensor.AddObservation(_Frozen); // 1

            // Obstacle related information
            if (_obstacleRecords.Count <= 0)
            {
                sensor.AddObservation(new float[4]);
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
                sensor.AddObservation(Dot(dirOfObstacle, -_obstacleRecords[0].transform.up.normalized)); // 1
            }
            
            // Targets related information
            if (_target == null)
            {
                sensor.AddObservation(new float[16]);
            }
            else
            {
                Vector3 agentPosition = transform.position;
                Vector3 targetPosition = _target.transform.position;
                Vector3 dirOfTarget = (targetPosition - agentPosition).normalized;
                float targetDistance = (targetPosition - agentPosition).magnitude;
                
                // relative distance to target
                sensor.AddObservation(targetDistance/_SearchRadius); // 1
                // Where is the target relative to the agent ( -1 means behind, left of, beneath. 1 means in front, right of, above)
                sensor.AddObservation(Dot(dirOfTarget, -_target.transform.forward.normalized)); // 1
                sensor.AddObservation(Dot(dirOfTarget, -_target.transform.right.normalized)); // 1
                sensor.AddObservation(Dot(dirOfTarget, -_target.transform.up.normalized)); // 1
                
                // The current direction the target is heading
                sensor.AddObservation(_target.velocity.normalized); // 3
                // The current velocity of the target
                sensor.AddObservation(_target.velocity.magnitude); // 1
                // The current direction of the target's angular velocity
                sensor.AddObservation(_target.angularVelocity.normalized); // 3
                // The current angular velocity of the target
                sensor.AddObservation(_target.angularVelocity.magnitude); // 1
                // The current direction the target is facing
                sensor.AddObservation(_target.rotation.normalized); // 3
                // The current mass of the target
                sensor.AddObservation(_target.mass); // 1
            }

        }

        public override void Heuristic(float[] actionsOut)
        {
            base.Heuristic(actionsOut);
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
            if (_isGrounded & vectorAction[0] > 0)
            {
                Vector3 move = new Vector3();
                
                // Z axis
                switch (vectorAction[1])
                {
                    // forwards movement
                    case 0:
                        move += forward * _MoveForce;
                        break;
                    // backwards movement
                    case 1:
                        move -= forward * _MoveForce;
                        break;
                }
                // X axis
                switch (vectorAction[2])
                {
                    // right movement
                    case 0:
                        move += right * _MoveForce;
                        break;
                    // left movement
                    case 1:
                        move -= right * _MoveForce;
                        break;
                }
                // we always jump when we move 
                // Y axis
                move += up * _JumpForce;

                // Apply move force
                _rigidBody.AddForce(move);
            }
            
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
