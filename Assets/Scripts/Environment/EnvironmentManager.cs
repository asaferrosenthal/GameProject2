using Traps;
using RunMan;
using UI;
using UnityEngine;

namespace Environment
{
    public class EnvironmentManager : MonoBehaviour
    {
        [Tooltip("The player object on a given level.")]
        public TheRunMan _RunMan;
        
        [Tooltip("The UI to be displayed on a level loading")]
        public GameObject _LevelStartUI;
        
        [Tooltip("The UI to be displayed on a level completion")]
        public GameObject _LevelEndUI;

        // Level object information
        private Spawner[] _spawners;
        private Trap[] _traps;

        // Level State information
        private LevelCountDownUI _levelCountDownUI;
        private EndOfLevelUI _endOfLevelUI;
        private bool _isGamePaused;
        
        // Score information
        private float _startTime;
        
        private void Awake()
        {
            _spawners = GetComponentsInChildren<Spawner>();
            _traps = GetComponentsInChildren<Trap>();
            _levelCountDownUI = Instantiate(_LevelStartUI).GetComponent<LevelCountDownUI>();
            _levelCountDownUI.SetManager(this);
            _endOfLevelUI = Instantiate(_LevelEndUI).GetComponent<EndOfLevelUI>();
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
            _startTime = 0;
        }

        public bool TogglePauseGame()
        {
            // if the countdown is occuring we cant toggle the game
            if (_levelCountDownUI.enabled | _endOfLevelUI.enabled) return false;
            
            _isGamePaused = !_isGamePaused;
            Time.timeScale = _isGamePaused ? 0 : 1;
            return _isGamePaused;
        }

        public void InitializeLevelStart()
        {
            Time.timeScale = 0;
            _isGamePaused = true;
        }
        
        public void LevelStart()
        {
            Time.timeScale = 1;
            _isGamePaused = false;
            // capture the current time
            _startTime = Time.time;
        }

        public void LevelEnd()
        {
            float score = 0;
            score = Time.time - _startTime;
            _endOfLevelUI.SetScore(score);
        }
        
    }
}
