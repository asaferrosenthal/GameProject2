using UnityEngine;

namespace Traps
{
    public class ExplosionTrap : Trap
    {
        [Tooltip("How much power is put into the explosion of the trap.")]
        public float _Power;

        public float _Radius;

        public float _UpwardsModifier;
        
        protected override void ApplyTrap()
        {
            if (!_Enabled) return;
            _Target.GetComponent<Rigidbody>().AddExplosionForce(_Power, _Target.transform.position, _Radius, _UpwardsModifier);
            _Enabled = false;
        }
    }
}
