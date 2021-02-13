using UnityEngine;

namespace Traps
{
    public class SpeedTrap : Trap
    {
        [Header("Velocity settings")]
        public float _ChangeFactor = -10f;
        protected override void ApplyTrap()
        {
            base.ApplyTrap();
            // Initialize variables for the affected rigidbody and forces
            Rigidbody body = _Target.GetComponent<Rigidbody>();
            
            Debug.Log(body.velocity);
            if (body == null) return;
            
            Vector3 newForce = body.velocity;
            
            
            Debug.Log("Adding a force to player " + newForce);
            
            // Create a new force based on the body's velocity and the change factor
            newForce *= _ChangeFactor;
            
            // Apply this new force
            body.AddForce(newForce);
            
            Debug.Log("The players new velocity " + body.velocity);
        }
    }
}
