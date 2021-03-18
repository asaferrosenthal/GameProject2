using System;
using TMPro;
using UnityEngine;
using Utility;

namespace Debugging
{
    public class DebuggerUI : MonoBehaviour
    {
        public Rigidbody _Player;
        public TextMeshProUGUI _VelocityUI;
        public TextMeshProUGUI _MassUI;
        public TextMeshProUGUI _CurrentMomentum;

        private float _oldVelocity;
        private void FixedUpdate()
        {
            float velocity = _Player.velocity.magnitude;
            _VelocityUI.text = velocity.ToString();
            _MassUI.text = _Player.mass.ToString();
            _CurrentMomentum.text = MomentumChecker.GetMomentum(_Player).ToString();
            _oldVelocity = velocity;
        }
    }
}
