using System;
using RunMan;
using Traps;
using UnityEngine;

namespace Adversary
{
    public class Goober : MonoBehaviour
    {
        private AdversaryAgent _agent;
        private Rigidbody _rigidbody;
        private Collider _collider;
        private Transform _parent;

        private void Awake()
        {
            _agent = GetComponent<AdversaryAgent>();
            _collider = GetComponent<Collider>();
            _rigidbody = GetComponent<Rigidbody>();
            _parent = transform;
        }

        private void Start()
        {
            AdversaryAgent.ONReset += Reset;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer != 10 | _agent._Frozen == true) return;
            
            _agent._Frozen = true;
            _agent.transform.SetParent(other.gameObject.transform);
            other.gameObject.GetComponent<TheRunMan>().AddMass(_rigidbody.mass);
            _collider.enabled = false;
            _rigidbody.isKinematic = true;
        }

        public void Reset()
        {
            _agent._Frozen = false;
            _agent.transform.SetParent(_parent);
            _collider.enabled = true;
            _rigidbody.isKinematic = false;
        }
    }
}
