using UnityEngine;

namespace Utility
{
    public static class MomentumChecker
    {
        public static float GetMomentum(Rigidbody body)
        {
            return (body.mass * body.velocity.magnitude);
        }
            
    }
}
