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
        
        protected override void ApplyTrap()
        {
            if (!_Enabled) return;
            ExplosionEffect();
            Rigidbody bod = _Target.GetComponent<Rigidbody>();
            if(bod != null) bod.AddExplosionForce(_Power, _Target.transform.position, _Radius, _UpwardsModifier);
        }
        
        void ExplosionEffect()
        {
            if (_Target.name == "RunMan")
            {
                _Explosion.Play();
            }
        }
    }
}
