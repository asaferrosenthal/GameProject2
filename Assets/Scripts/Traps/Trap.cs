using System;
using UnityEngine;

namespace Traps
{
    public class Trap : MonoBehaviour
    {
        [Tooltip("Can this trap be activated.")]
        public bool _Enabled = true;
        [Tooltip("This trap is activated when objects stay in it.")]
        public bool _TriggersOnStay;
        [Tooltip("This trap is activated when object enter into it.")]
        public bool _TriggersOnEnter;
        [Tooltip("This trap is activated when objects exit it.")]
        public bool _TriggersOnExit;
        [Tooltip("The audio source played on a trigger of this trap.")]
        public AudioSource _audioSource;
        
        protected GameObject _Target;

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
                _audioSource.volume = 1;
            }
            else
            {
                _audioSource.volume = 0;

            }
            
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
            _audioSource.Play();
        }

        public virtual void ResetTrap()
        {
            gameObject.SetActive(true);
            _Enabled = true;
        }
    }
}
