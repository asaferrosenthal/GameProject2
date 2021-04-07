using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Main_Menu
{
  public class StartMenu : MonoBehaviour
  {
    public GameObject _EscapeMenu;

    private void Awake()
    {
      // ensure the cursor can't leave the window
      Cursor.lockState = CursorLockMode.None;
    }

    private void OnEnable()
    {
      Cursor.visible = true;
    }

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
