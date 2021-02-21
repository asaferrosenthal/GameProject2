using System;
using Environment;
using UnityEngine;

namespace RunMan
{
    /// <summary>
    /// Class that MAY eventually come into use for level transitions/player resets.
    /// Could do level specific settings for player that are external to prefabs?
    /// </summary>
    public class PlayerManager : MonoBehaviour
    {
        public Spawner _PlayerSpawner;
        private TheRunMan _theRunMan;
        private void Start()
        {
            _theRunMan = FindObjectOfType<TheRunMan>(); // very inefficient
        }

        public void ResetPlayer()
        {
            _theRunMan.ResetRunMan();
            _PlayerSpawner.ResetObjectPositions();
        }
    }
}
