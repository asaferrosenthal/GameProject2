using System;
using RunMan;
using Traps;
using UnityEngine;

namespace Adversary
{
    public class Goober : InteractionBase
    {
        [SerializeField] private int _PlayerLayer = 10;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer != _PlayerLayer | _Agent._Frozen) return;
            _Agent._Frozen = true;
            other.gameObject.GetComponent<TheRunMan>().AddMass(_Rigidbody.mass);
            _Collider.enabled = false;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.layer != _PlayerLayer | _Agent._Frozen) return;
            _Agent._Frozen = true;
            other.gameObject.GetComponent<TheRunMan>().AddMass(_Rigidbody.mass);
            _Collider.enabled = false;
        }
    }
}
