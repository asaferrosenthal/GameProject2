using UnityEngine;
using UnityEngine.SceneManagement;

namespace Main_Menu
{
  public class StartMenu : MonoBehaviour
  {
    public GameObject _EscapeMenu;
    public void Exit() 
    {
      Application.Quit();
    }

    public void Play()
    {
      // 0 index will always be the start menu, 1 will be level 1
      SceneManager.LoadScene(1);
    }

  }
}
