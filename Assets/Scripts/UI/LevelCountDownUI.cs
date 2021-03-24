using System;
using Environment;
using UnityEngine;

namespace UI
{
    public class LevelCountDownUI : MonoBehaviour
    {
        [Tooltip("The animation that will be played on level start.")]
        public Animator _CountDown;
        
        // Variable for storing a scenes manager
        private EnvironmentManager _manager;
        

        public void SetManager(EnvironmentManager manager)
        {
            _manager = manager;
            _manager.CountDownStart();
        }

        private void OnEnable()
        {

        }

        private void OnDisable()
        {
            _manager.LevelStart();
        }
    }
}
