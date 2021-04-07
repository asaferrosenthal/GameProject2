using System;
using UnityEngine;
using UnityEngine.Animations;
using Utility;

namespace RunMan
{
    public class PlayerAnimationController : MonoBehaviour
    {
        public Animator _Animator;
        public float _animationMultiplier;
        
        public TheRunMan _RunMan;
        private Rigidbody _body;

        private bool _running = false;
        private bool _inAir = false;

        private void Awake()
        {
            _body = _RunMan.GetComponent<Rigidbody>();
            _Animator.SetBool("running", false);
        }

        private void OnEnable()
        {
            _Animator.SetBool("running", false);
            _Animator.SetBool("inAir", false);
        }

        private void FixedUpdate()
        {
            float animSpeed = 0;
            animSpeed = MomentumChecker.GetMomentum(_body) * _animationMultiplier;
            _Animator.SetFloat("runSpeed", animSpeed);
            
            // check if we are falling
            if (Mathf.Abs(_body.velocity.y) > 1f)
            {
                _inAir = true;
                _Animator.SetBool("inAir", _inAir);
            }
            else
            {
                _inAir = false;
                _Animator.SetBool("inAir", _inAir);
            }
            
            // check if we are running
            if (MomentumChecker.GetMomentum(_body) > .1f)
            {
                if (_running) return;
                _running = true;
                _Animator.SetBool("running", _running);
            }
            else // then we are not running
            {
                if (!_running) return;
                _running = false;
                _Animator.SetBool("running", _running);
            }
            
        }

        public void GoIdle()
        {
            _running = false;
            _inAir = false;
            _Animator.SetBool("running", _running);
            _Animator.SetBool("inAir", _inAir);
        }
    
    }
}
