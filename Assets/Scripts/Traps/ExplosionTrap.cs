using System;
using UnityEngine;

namespace Traps
{
    public class ExplosionTrap : Trap
    {
        [Tooltip("How much power is put into the explosion of the trap.")]
        public float _Power;

        public float _Radius;

        public float _UpwardsModifier;
        
        public ParticleSystem _Explosion;
        
        private void Awake()
        {
            _Explosion.Stop(true);
        }

        protected override void ApplyTrap()
        {
            if (!_Enabled) return;
            _Explosion.Play(true);
            Rigidbody bod = _Target.GetComponent<Rigidbody>();
            if(bod != null) bod.AddExplosionForce(_Power, _Target.transform.position, _Radius, _UpwardsModifier);
        }
        
    }
}
