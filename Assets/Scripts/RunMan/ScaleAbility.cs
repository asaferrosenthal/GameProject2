using UnityEngine;

namespace RunMan
{
    /// <summary>
    /// Scale Ability is responsible for the Run Man mechanic of the size increase/decrease mechanic that also affects mass
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class ScaleAbility : MonoBehaviour
    {
        [Header("Run Man Settings")]
        [Tooltip("Factor by which inputs change Run Man scale")]
        public float _ScaleFactor = 0.2f;

        // record of rigidbody to prevent need to re-access
        private Rigidbody _rigidBody;

        // value used for scaling Run Man object
        private float _currentScale = MinScale;

        // Use the on awake scale value as the default Run Man scale
        private Vector3 _defaultScale;

        // Use the on awake mass value as default
        private float _defaultMass;

        // Set maximum and minimum scaling of run man, could make these public for testing
        private const float MaxScale = 1.5f;
        private const float MinScale = 1f;

        // Initializes defaults from the rigidbody attached to a shared game object
        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody>();
            _defaultScale = transform.localScale;
            _defaultMass = _rigidBody.mass;
        }

        private void Update()
        {
            UpdateRunManScale();
            ScaleRunMan();
        }

        /// <summary>
        /// Uses exclusively mousewheel input to scale Run Man up and down, could modify Scroll input settings for additional inputs
        /// </summary>
        private void UpdateRunManScale()
        {
            _currentScale = Mathf.Clamp(_currentScale + (Input.GetAxis("Scroll") * _ScaleFactor), MinScale, MaxScale);
        }

        /// <summary>
        /// Apply the current scale to the actual game object of Run Man
        /// </summary>
        private void ScaleRunMan()
        {
            transform.localScale = _defaultScale * _currentScale;
            _rigidBody.mass = _defaultMass * _currentScale;
        }
    }
}