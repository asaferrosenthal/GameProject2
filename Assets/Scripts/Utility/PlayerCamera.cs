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
            
            if(Time.timeScale != 0) transform.eulerAngles += rotation;
            
            transform.eulerAngles = new Vector3(ClampAngle(transform.eulerAngles.x,-20,45), transform.eulerAngles.y, transform.eulerAngles.z);
        }

        public void Reset()
        {
            transform.eulerAngles = _Target.transform.eulerAngles;
        }
        
        private float ClampAngle(float angle, float min,float max)
        {
            if (angle<90 || angle>270)
            {       
                // if angle in the critic region...
                if (angle>180) angle -= 360;  // convert all angles to -180..+180
                if (max>180) max -= 360;
                if (min>180) min -= 360;
            }    
            angle = Mathf.Clamp(angle, min, max);
            if (angle<0) angle += 360;  // if angle negative, convert to 0..360
            return angle;
        }
    }
}