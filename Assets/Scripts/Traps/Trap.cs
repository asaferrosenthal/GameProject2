using System;
using UnityEngine;

namespace Traps
{
    public class Trap : MonoBehaviour
    {
        protected GameObject _Target;
        
        protected internal virtual void OnCollisionStay(Collision other)
        {
            _Target = other.gameObject;
            ApplyTrap();
        }

        protected internal void OnTriggerStay(Collider other)
        {
            _Target = other.gameObject;
            ApplyTrap();
        }

        protected virtual void ApplyTrap()
        {
            Debug.Log("A trap has been applied!");
        }
    }
}
