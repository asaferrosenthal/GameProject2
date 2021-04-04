using System;
using UnityEngine;
using System.Collections;
using Traps;
using UnityEngine.SceneManagement;

namespace Environment
{
  
  public class EndTrigger : Trap
  {
      public EnvironmentManager _Manager;
      private const int PlayerLayer = 10;
      private Coroutine _levelEnd;

      protected override void OnTriggerEnter(Collider other)
      {
          if (other.gameObject.layer == PlayerLayer && _levelEnd == null)
          {
              // play end of level audio
              _AudioSource.Play();
              _levelEnd = StartCoroutine(_Manager.LevelEnd());
          }
      }

      public override void ResetTrap()
      {
          _levelEnd = null;
      }
  }

}
