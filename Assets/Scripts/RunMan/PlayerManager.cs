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
        private ScaleAbility _runMan;
        private void Start()
        {
            _runMan = FindObjectOfType<ScaleAbility>(); // very inefficient
        }

        public void ResetPlayer()
        {
            _runMan.Reset();
            _PlayerSpawner.ResetObjectPositions();
        }
    }
}
