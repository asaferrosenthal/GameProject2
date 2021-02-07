using System;
using UnityEngine;

namespace RunMan
{
    public class PlayerManager : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        private ScaleAbility _runMan;
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _runMan = GetComponent<ScaleAbility>();
        }

        public void ResetPlayer()
        {
            _rigidbody.velocity = Vector3.zero;
            //_runMan.ResetScale;
        }
    }
}
