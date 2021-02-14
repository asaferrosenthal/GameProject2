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
        //public bool _growBalloon = false;
        protected GameObject _Target;
        
        protected internal void OnTriggerStay(Collider other)
        {
            if (!_TriggersOnStay) return;
            _Target = other.gameObject;
            ApplyTrap();
            _Enabled = true;
        }

        protected internal void OnTriggerEnter(Collider other)
        {
            if (!_TriggersOnEnter) return;
            _Target = other.gameObject;
            ApplyTrap();
            _Enabled = true;
        }

        protected internal void OnTriggerExit(Collider other)
        {
            if (!_TriggersOnExit) return;
            _Target = other.gameObject;
            ApplyTrap();
            _Enabled = false;
        }

        protected virtual void ApplyTrap()
        {
            if (!_Enabled) return;
            
            Debug.Log("A trap has been applied!");
        }
    }
}
