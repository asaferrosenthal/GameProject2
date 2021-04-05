using System.Collections;
using Traps;
using RunMan;
using UI;
using UnityEngine;
using UnityEngine.UI;

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

        // the in use movement settings
        private MovementControls _movement;
        
        // Level object information
        private Spawner[] _spawners;
        private Trap[] _traps;

        // Level State information
        private LevelCountDownUI _levelCountDownUI;
        private EndOfLevelUI _endOfLevelUI;
        private EscapeUI _escapeUI;
        private bool _isGamePaused;
        
        // Score information
        private float _startTime;

        // other
        public bool _Training;

        // Sound Effects
        public AudioSource _AudioSource;
        public AudioClip _Complete;

        private void Awake()
        {
            _AudioSource = GetComponent<AudioSource>();
            _spawners = GetComponentsInChildren<Spawner>();
            _traps = GetComponentsInChildren<Trap>();
            

            if (_Training) return;
            // initialize level start count down
            _levelCountDownUI = Instantiate(_LevelStartUI).GetComponent<LevelCountDownUI>();
            _levelCountDownUI.SetManager(this);
            InitializeLevelStart();
            
            // initialize end game menu
            _endOfLevelUI = Instantiate(_LevelEndUI).GetComponent<EndOfLevelUI>();
            _endOfLevelUI.SetManager(this);
            _endOfLevelUI.gameObject.SetActive(false);
            
            // initialize escape menu
            _escapeUI = FindObjectOfType<EscapeUI>();
            _escapeUI._Manager = this;
            
            //_escapeUI.UpdateSensitivity();
            _escapeUI.UpdateScale();
            
            // get the movement controls
            if (_RunMan != null) _movement = _RunMan.GetComponent<MovementControls>();
        }

        public void ResetEnvironment()
        {
            foreach (Spawner ele in _spawners)
            {
                ele.ResetObjectPositions();
            }

            foreach (Trap ele in _traps)
            {
                ele.ResetTrap();
            }

            if (_Training) return;

            if (_RunMan != null) _RunMan.ResetRunMan();

            _levelCountDownUI.enabled = true;

            _startTime = 0;
        }

        public bool TogglePauseGame()
        {
            // if the countdown is occuring we cant toggle the game
            if (_levelCountDownUI.enabled | _endOfLevelUI.gameObject.activeSelf) return false;
            
            _isGamePaused = !_isGamePaused;
            Time.timeScale = _isGamePaused ? 0 : 1;
            Cursor.visible = _isGamePaused;

            return _isGamePaused;
        }

        public void InitializeLevelStart()
        {
            Debug.Log("INIT LevelStarted");
            Time.timeScale = 0;
            _isGamePaused = true;
            
        }
        
        public void LevelStart()
        {
            Debug.Log("LevelStarted");
            Time.timeScale = 1;
            _isGamePaused = false;
            // capture the current time
            _startTime = Time.time;
        }
        
        public IEnumerator LevelEnd()
        {
            // slow the player and stop forward propulsion
            _RunMan._Movement._StopPlayer = true;
            _RunMan.AlterVelocity(.1f);

            yield return new WaitForSeconds(1f);
            
            // idle the runman
            _RunMan._AnimationController.GoIdle();
            
            // pause game
            TogglePauseGame();
            _endOfLevelUI.gameObject.SetActive(true);

            // Build Score value
            float score = 0;
            score = Time.time - _startTime;
            _endOfLevelUI.SetScore(score);
        }

        public void UpdateMouseSense(float num)
        {
            _movement._DefaultRotationSpeed = num;
        }

        public float GetMouseSense()
        {
            return _movement._DefaultRotationSpeed;
        }

        public void UpdateMouseScale(float num)
        {
            _RunMan._ScaleFactor = num;
        }

        public float GetMouseScale()
        {
            return _RunMan._ScaleFactor;
        }
    }
}
