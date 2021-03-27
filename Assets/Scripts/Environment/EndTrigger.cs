using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Environment
{
    
    public class EndTrigger : MonoBehaviour
    {
        public EnvironmentManager _Manager;
        private const int PlayerLayer = 10;
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == PlayerLayer) StartCoroutine(_Manager.LevelEnd());
        }
    }

}
