using System;
using UnityEngine;

namespace Traps
{
    public class SpeedTrap : Trap
    {
        [Header("Velocity settings")]
        public float _ChangeFactor = 0f;
        protected override void ApplyTrap()
        {
            base.ApplyTrap();
            
            // Initialize variables for the affected rigidbody and forces
            Rigidbody body = _Target.GetComponent<Rigidbody>();
            
            if (body == null) return;
            
            Vector3 newForce = body.velocity;

            // Create a new force based on the body's velocity and the change factor
            newForce *= _ChangeFactor;
            
            // Apply this new force
            body.AddForce(newForce);
        }

        protected override void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.layer == 10) _AudioSource.Play();
        }
    }
}
