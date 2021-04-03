using System;
using UnityEngine;

namespace Environment
{
    /*
     * This class is responsible for turning off non-required assets as they fall off the map. Also triggers the rest of the game if the player hits it.
     */
    public class DeathField : MonoBehaviour
    {
        [SerializeField] private int _PlayerLayer = 10;
        [SerializeField] private EnvironmentManager _Manager;

        public AudioSource audioSource;

        void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == _PlayerLayer)
            {
                audioSource.Play();
                _Manager.ResetEnvironment();
                return;
            }
            
            other.gameObject.SetActive(false);
        }
    }
}
