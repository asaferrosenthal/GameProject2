using System;
using Traps;
using UnityEngine;
using Utility;

namespace Environment
{
    public class MomentumValidator : MonoBehaviour
    {
        public Material _valid;

        public Material _invalid;
        // Particle indicator
        private Renderer _indicator;
        private float _threshHold;

        private const int PlayerLayer = 10;
        private void Awake()
        {
            _indicator = GetComponent<Renderer>();
            _threshHold = GetComponentInParent<DestructableWall>()._BreakThreshold;
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.layer != PlayerLayer) return;
            
            _indicator.material = MomentumChecker.GetMomentum(other.attachedRigidbody) >= _threshHold ? _valid : _invalid;
        }

        private void OnTriggerExit(Collider other)
        {
            _indicator.material = _invalid;
            _indicator.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            _indicator.enabled = true;
        }
    }
}
