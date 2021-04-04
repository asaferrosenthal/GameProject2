using System;
using System.Collections;
using UnityEngine;
using Utility;

namespace Environment
{
    /*
     * This class is responsible for turning off non-required assets as they fall off the map. Also triggers the rest of the game if the player hits it.
     */
    public class DeathField : MonoBehaviour
    {
        public PlayerCamera _PlayerCam;
        [SerializeField] private int _PlayerLayer = 10;
        [SerializeField] private EnvironmentManager _Manager;

        public AudioSource _AudioSource;

        private Coroutine _DeathRoutine;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == _PlayerLayer)
            {
                _DeathRoutine = StartCoroutine(DeathRoutine());
                return;
            }
            
            other.gameObject.SetActive(false);
        }

        private IEnumerator DeathRoutine()
        {
            _AudioSource.Play();
            _PlayerCam.enabled = false;
            yield return new WaitForSeconds(2f);
            _PlayerCam.enabled = true;
            _Manager.ResetEnvironment();
        }
    }
}
