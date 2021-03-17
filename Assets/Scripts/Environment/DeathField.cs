using System;
using UnityEngine;

namespace Environment
{
    public class DeathField : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            OnEnter();
        }

        private void OnCollisionEnter(Collision other)
        {
            OnEnter();
        }

        private void OnEnter()
        {
            
        }
    }
}