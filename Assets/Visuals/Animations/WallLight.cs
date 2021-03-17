using System;
using UnityEngine;

namespace Traps
{
    public class WallLight : MonoBehaviour
    {
        private float _BreakThreshold;
        private float _momentum;
        [Tooltip("Drag the Destructible Wall object here")]
        public GameObject wallObject;

        private Renderer _lightColour;
        public Color stopColour = Color.red;
        public Color goColour = Color.green;

        [Tooltip("Drag the Run Man object here")]
        public Rigidbody RunMan;

        private void Start()
        {
            _lightColour = GetComponent<Renderer>();
            getBreakThreshold();
        }

        private void getBreakThreshold()
        {
            DestructableWall wall = wallObject.GetComponentInParent<DestructableWall>();
            _BreakThreshold = wall._BreakThreshold; 
        }

        private void Update()
        {
            checkMomentum();
            changeLight();
        }

        private void checkMomentum()
        {
            float velocity = RunMan.velocity.magnitude;
            float mass = RunMan.mass;
            _momentum = mass * velocity;
        }

        private void changeLight()
        {
            if (_momentum > _BreakThreshold)
            {
                _lightColour.material.SetColor("_EmissionColor", goColour);
            }

            else
            {
                _lightColour.material.SetColor("_EmissionColor", stopColour);
            }
        }
    }
}
