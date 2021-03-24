using System;
using Adversary;
using Traps;
using RunMan;
using UI;
using UnityEngine;

namespace Environment
{
    public class EnvironmentManager : MonoBehaviour
    {
        public TheRunMan _RunMan;
        public GameObject _LevelStartUI;
        public bool _Training;
        
        private Spawner[] _spawners;
        private Trap[] _traps;

        private LevelCountDownUI _levelCountDownUI;
        private bool _isGamePaused;
        
        private void Awake()
        {
            _spawners = GetComponentsInChildren<Spawner>();
            _traps = GetComponentsInChildren<Trap>();
            _levelCountDownUI = Instantiate(_LevelStartUI).GetComponent<LevelCountDownUI>();
            _levelCountDownUI.SetManager(this);
        }

        public void ResetEnvironment()
        {
            foreach(Spawner ele in _spawners)
            {
                ele.ResetObjectPositions();
            }

            foreach (Trap ele in _traps)
            {
                ele.ResetTrap();
            }

            if (_RunMan != null) _RunMan.ResetRunMan();
            _levelCountDownUI.enabled = true;
        }

        public bool TogglePauseGame()
        {
            // if the countdown is occuring we cant toggle the game
            if (_levelCountDownUI.enabled) return false;
            
            _isGamePaused = !_isGamePaused;
            Time.timeScale = _isGamePaused ? 0 : 1;
            return _isGamePaused;
        }

        public void CountDownStart()
        {
            Debug.Log("PLAYING ANIMATION");
            
            Time.timeScale = 0;
            _isGamePaused = true;
        }
        public void LevelStart()
        {
            Time.timeScale = 1;
            _isGamePaused = false;
        }

        
    }
}
