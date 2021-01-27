using UnityEngine;

namespace Utility
{
    /// <summary>
    /// Follow Object class follows an assigned target object
    /// </summary>
    public class FollowObject : MonoBehaviour
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
        private void FixedUpdate()
        {
            var position = _Target.transform.position;
            transform.localPosition = Vector3.Lerp(transform.localPosition,
                new Vector3(_Offset.x, _Offset.y, _Offset.z), Time.deltaTime * _Speed);
        }
    }
}