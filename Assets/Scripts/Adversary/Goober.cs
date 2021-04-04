using System;
using RunMan;
using Traps;
using UnityEngine;

namespace Adversary
{
    public class Goober : InteractionBase
    {
        public MeshRenderer _MeshRenderer;
        public SphereCollider _Collider;
        public AudioSource _Audio;
        public GameObject _eyes;
        [SerializeField] private int _PlayerLayer = 10;

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.layer != _PlayerLayer | _Agent._Frozen) return;
            _Audio.Play();
            _Agent._Frozen = true;
            other.gameObject.GetComponent<TheRunMan>().AddMass(_Rigidbody.mass);
            _MeshRenderer.enabled = false;
            _Collider.enabled = false;
            _eyes.SetActive(false);
        }

        public override void Reset()
        {
            _MeshRenderer.enabled = true;
            _Collider.enabled = true;
            _eyes.SetActive(true);

        }
    }
}
