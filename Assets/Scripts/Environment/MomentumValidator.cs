using System;
using Traps;
using UnityEngine;
using Utility;

namespace Environment
{
    public class MomentumValidator : MonoBehaviour
    {
        public Material _Valid;

        public Material _Invalid;
        
        // Particle indicator
        public Renderer _Indicator;
        public DestructableWall _Wall;
        private float _threshHold;

        private const int PlayerLayer = 10;
        private void Awake()
        {
            _Indicator = GetComponent<Renderer>();
            _threshHold = _Wall._BreakThreshold;
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.layer != PlayerLayer) return;
            
            _Indicator.material = MomentumChecker.GetMomentum(other.attachedRigidbody) >= _threshHold ? _Valid : _Invalid;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer != PlayerLayer) return;
            
            _Indicator.material = _Invalid;
            _Indicator.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer != PlayerLayer) return;
            _Indicator.enabled = true;
        }
    }
}
