using TMPro;
using UnityEngine;
using Utility;

namespace UI
{
    public class PlayerUI : MonoBehaviour
    {
        public Rigidbody _Player;
        public TextMeshProUGUI _VelocityUI;
        public TextMeshProUGUI _MassUI;
        public TextMeshProUGUI _CurrentMomentum;

        private float _oldVelocity;
        private void Update()
        {
            float velocity = _Player.velocity.magnitude;
            _VelocityUI.text = velocity.ToString("#.00");
            _MassUI.text = _Player.mass.ToString("#.00");
            _CurrentMomentum.text = MomentumChecker.GetMomentum(_Player).ToString("#.00");
            _oldVelocity = velocity;
        }
    }
}
