using UnityEngine;

namespace Adversary
{
    public class InteractionBase : MonoBehaviour
    {
        protected AdversaryAgent _Agent;
        protected Rigidbody _Rigidbody;
        protected Collider _Collider;
        protected Transform _Parent;

        private void Awake()
        {
            // required components
            _Agent = GetComponent<AdversaryAgent>();
            _Collider = GetComponent<Collider>();
            _Rigidbody = GetComponent<Rigidbody>();
            // object reset information
            var transform1 = transform;
            _Parent = transform1;

        }
        
        public virtual void Reset()
        {
            _Collider.enabled = true;
        }
        
    }
}
