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
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
