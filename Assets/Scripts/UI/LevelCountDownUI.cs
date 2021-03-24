using System;
using UnityEngine;

namespace UI
{
    public class LevelCountDownUI : MonoBehaviour
    {
        private void Awake()
        {
            Time.timeScale = 0f;
        }

        private void OnDisable()
        {
            Time.timeScale = 1f;
        }
    }
}
