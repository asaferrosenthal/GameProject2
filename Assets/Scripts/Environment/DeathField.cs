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
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == _PlayerLayer)
            {
                _Manager.ResetEnvironment();
                return;
            }
            
            other.gameObject.SetActive(false);
        }
    }
}
