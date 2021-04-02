using Environment;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    
    
    public class LevelUIBase : MonoBehaviour
    {
        // Variable for storing a scenes manager
        protected EnvironmentManager _Manager;

        public void SetManager(EnvironmentManager manager)
        {
            _Manager = manager;
        }

        public void NextLevel()
        {
            int index = SceneManager.GetActiveScene().buildIndex + 1;
            SceneManager.LoadScene(index >= SceneManager.sceneCountInBuildSettings ? 0 : index);
        }

        public void RestartLevel()
        {
            _Manager.ResetEnvironment();
            gameObject.SetActive(false);
        }
        
        public void ReturnToStartMenu()
        {
            SceneManager.LoadScene(0);
        }
    }
}
