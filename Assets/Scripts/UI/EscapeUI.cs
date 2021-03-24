using System;
using System.Collections.Generic;
using Environment;
using TMPro;
using UnityEditor;
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
        private EnvironmentManager _manager;
        // Level selection drop down
        private TMP_Dropdown _dropdown;
        private void Awake()
        {
            _dropdown = GetComponentInChildren<TMP_Dropdown>();
            _PageSpace.SetActive(false);
            LoadDropDown();
            DontDestroyOnLoad(this);
        }

        // Listen for delegate OnSceneLoaded 
        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        // Stop listening for OnSceneLoaded
        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
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
            _PageSpace.SetActive(!_PageSpace.activeSelf);
            if (_manager != null) _manager.TogglePauseGame();
        }
        
        // Evokes environment manager level reset and turns off the escape menu
        public void RestartLevel()
        {
            if (_manager != null) _manager.ResetEnvironment();
            ToggleEscapeMenu();
        }
        
        // Returns game to index 0 of build index
        public void ReturnToStartMenu()
        {
            SceneManager.LoadScene(0);
            ToggleEscapeMenu();
            Destroy(this.gameObject);
        }

        // utility for going to a specific scene
        public void SelectScene(SceneAsset scene)
        {
            SceneManager.LoadScene(scene.name);
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

        // Delegate for scene loading
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Debug.Log(scene.name + " was loaded successfully.");
            _manager = (EnvironmentManager) FindObjectOfType(typeof(EnvironmentManager));
            Debug.Log(_manager + " was found.");
        }
        
    }
}
