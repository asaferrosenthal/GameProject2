using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxManager : MonoBehaviour
{
  public AudioSource Audio;

  public AudioClip Click;

  public static SfxManager sfxInstance;

  private void Awake()
  {
    if (sfxInstance != null && sfxInstance != this) {
      Destroy(this.gameObject);
      return;
    }

    sfxInstance = this;
    DontDestroyOnLoad(gameObject);
    GetComponent<AudioSource>().Play();
    PlayerPrefs.SetFloat("volume", 1);

  }

  // Update is called once per frame
  void Update()
  {
      GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("volume");
  }
}
