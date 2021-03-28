using System;
using UnityEngine;
using UnityEngine.Animations;
using Utility;

namespace RunMan
{
    public class PlayerAnimationController : MonoBehaviour
    {
        public Animator _Animator;

        public TheRunMan _RunMan;
        private Rigidbody _body;

        private bool _running = false;

        private void Awake()
        {
            _body = _RunMan.GetComponent<Rigidbody>();
            _Animator.SetBool("running", false);
        }

        private void OnEnable()
        {
            _Animator.SetBool("running", false);
        }

        private void FixedUpdate()
        {
            if (Mathf.Abs(_body.velocity.y) > .5f)
            {
                _running = false;// in air
                _Animator.SetBool("running", _running);
                return;
            }

            if (MomentumChecker.GetMomentum(_body) > .1f)
            {
                if (_running) return;
                _running = true;
                _Animator.SetBool("running", _running);
            }
            else
            {
                if (!_running) return;
                _running = false;
                _Animator.SetBool("running", _running);
            }
            
        }

        public void GoIdle()
        {
            _running = false;
            _Animator.SetBool("running", _running);
        }
    
    }
}
