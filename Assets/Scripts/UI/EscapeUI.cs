using System;
using System.Collections.Generic;
using Environment;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    
    public class EscapeUI : MonoBehaviour
    {
        // The interactable area of the escape menu
        public GameObject _PageSpace;
        
        // Variable for storing a scenes manager
        public EnvironmentManager _Manager;
        
        // Level selection drop down
        private TMP_Dropdown _dropdown;

        [Tooltip("Sensitivity field")]
        public TMP_InputField _SensitivityInputField;
        
        [Tooltip("Sensitivity field")]
        public Slider _ScaleSlider;
        
        private void Awake()
        {
            _dropdown = GetComponentInChildren<TMP_Dropdown>();
            _PageSpace.SetActive(false);
            LoadDropDown();
            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            GetMouseSensitivity();
        }

        private void Update()
        {
            // Open the escape menu if escape is pressed AND we are not on the start menu
            if (Input.GetKeyUp(KeyCode.Escape) & SceneManager.GetActiveScene().buildIndex != 0)
            {
                ToggleEscapeMenu();
            }
        }

        // toggles on and off the Escape key menu
        public void ToggleEscapeMenu()
        {
            bool isPaused = false;
            // record if the game is paused or not
            if (_Manager != null) isPaused = _Manager.TogglePauseGame();
            // if the game is paused than show the escape UI
            _PageSpace.SetActive(isPaused);
        }
        
        // Evokes environment manager level reset and turns off the escape menu
        public void RestartLevel()
        {
            if (_Manager != null) _Manager.ResetEnvironment();
            ToggleEscapeMenu();
        }
        
        // Returns game to index 0 of build index
        public void ReturnToStartMenu()
        {
            SceneManager.LoadScene(0);
            ToggleEscapeMenu();
            Destroy(this.gameObject);
        }
        
        // Takes currently selected option and loads it
        public void LoadDropDownScene()
        {
            int index = 0;
            int.TryParse(_dropdown.captionText.text, out index);
            ToggleEscapeMenu();
            SceneManager.LoadScene(index);
        }

        // Builds the option list for the escape menu based on the Build Index in build settings
        private void LoadDropDown()
        {
            int sceneCount = SceneManager.sceneCountInBuildSettings;
            
            List<TMP_Dropdown.OptionData> optionList = new List<TMP_Dropdown.OptionData>();
            for (int i = 1; i < sceneCount; i ++)
            {
                TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
                option.text = i.ToString();
                optionList.Add(option);
            }

            _dropdown.options = optionList;
        }

        public void UpdateSensitivity()
        {
            var input = Single.Parse(_SensitivityInputField.text);
            if (input == null) return;
            _Manager.UpdateMouseSense(input);
        }

        public void GetMouseSensitivity()
        {
             _SensitivityInputField.text = _Manager.GetMouseSense().ToString();
        }

        public void UpdateScale()
        {
            _Manager.UpdateMouseScale(_ScaleSlider.value);
        }

        public void GetScale()
        {
            _ScaleSlider.value = _Manager.GetMouseScale();
        }

    }
}
