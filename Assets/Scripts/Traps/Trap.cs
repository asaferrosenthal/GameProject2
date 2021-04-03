using System;
using UnityEngine;

namespace Traps
{
    public class Trap : MonoBehaviour
    {
        public bool _Enabled = true;
        public bool _TriggersOnStay;
        public bool _TriggersOnEnter;
        public bool _TriggersOnExit;
        protected GameObject _Target;
        public AudioSource audioSource;

        void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        protected internal void OnTriggerStay(Collider other)
        {
            if (!_TriggersOnStay) return;
            _Target = other.gameObject;
            ApplyTrap();
        }

        protected internal void OnTriggerEnter(Collider other)
        {

            if (!_TriggersOnEnter) return;
            _Target = other.gameObject;

            if (Time.timeSinceLevelLoad != 0)
            {
                audioSource.volume = 1;
            }
            else
            {
                audioSource.volume = 0;

            }

            audioSource.Play();
            ApplyTrap();

        }

        protected internal void OnTriggerExit(Collider other)
        {
            if (!_TriggersOnExit) return;
            _Target = other.gameObject;
            ApplyTrap();
        }

        protected virtual void ApplyTrap()
        {
            if (!_Enabled) return;
            //Debug.Log(_Target.name + " has triggered the trap");
        }

        public virtual void ResetTrap()
        {
            gameObject.SetActive(true);
            _Enabled = true;
        }
    }
}
