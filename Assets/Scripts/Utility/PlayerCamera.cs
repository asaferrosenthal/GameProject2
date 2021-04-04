using System;
using UnityEngine;

namespace Utility
{
    /// <summary>
    /// Follow Object class follows an assigned target object
    /// </summary>
    public class PlayerCamera : MonoBehaviour
    {
        [Tooltip("The game object to be followed by this object")]
        public GameObject _Target;

        [Tooltip("How quickly the camera moves towards the targets position")]
        public float _Speed = 1;

        [Tooltip("Offset this object has from the target objects transform")]
        public Vector3 _Offset;

        /// <summary>
        /// Non-linearly translates from local object's current position to a target object's position.
        /// This script could be expanded to have toggles choosing the style of non-linear transition.
        /// </summary>
        private void Update()
        {
            var position = _Target.transform.position;
            
            transform.position = Vector3.Lerp(transform.position, position + _Offset, Time.unscaledTime * _Speed);
            
            Vector3 rotation = new Vector3(( -1 * Input.GetAxis("Mouse Y")), (Input.GetAxis("Mouse X")), 0);
            
            transform.eulerAngles += rotation;
        }

        public void Reset()
        {
            transform.eulerAngles = _Target.transform.eulerAngles;
        }
    }
}