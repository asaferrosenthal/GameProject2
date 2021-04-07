using System;
using UnityEngine;

namespace Utility
{
    public class FloatUpAndDown : MonoBehaviour
    {
        public float _Amplitude;
        public float _Speed;

        private float _originalYPos;

        private void Awake()
        {
            _originalYPos = transform.localPosition.y;
        }

        private void FixedUpdate()
        {
            transform.localPosition = new Vector3(transform.localPosition.x, _Amplitude * Mathf.Sin(Time.time * _Speed) + _originalYPos, transform.localPosition.z);
        }
    }
}
