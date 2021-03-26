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
            _Manager.InitializeLevelStart();
        }

        protected void NextLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
