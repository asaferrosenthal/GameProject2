using RunMan;
using UnityEditor.Animations;
using UnityEngine;
using Utility;

public class PlayerAnimationController : MonoBehaviour
{
    public Animator _Animator;
    public AnimatorController _Idle;
    public AnimatorController _Run;

    public TheRunMan _RunMan;
    private Rigidbody _body;

    private bool _running = false;
    private void Awake()
    {
        _body = _RunMan.GetComponent<Rigidbody>();
        _Animator.runtimeAnimatorController = _Idle;
    }

    private void FixedUpdate()
    {
        if (Mathf.Abs(_body.velocity.y) > 1f)
        {
            _Animator.runtimeAnimatorController = _Idle;
            _running = false;// in air
            return;
        }

        if (MomentumChecker.GetMomentum(_body) > .1f)
        {
            if (_running) return;
            _Animator.runtimeAnimatorController = _Run;
            _running = true;
        }
        else
        {
            if (!_running) return;
            _Animator.runtimeAnimatorController = _Idle;
            _running = false;
        }
        

    }
    
}
