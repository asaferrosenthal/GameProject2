using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class EndOfLevelUI : LevelUIBase
    {
        public TextMeshProUGUI _ScoreUI;

        public void SetScore(float num)
        {
            _ScoreUI.text = num.ToString("#.");
        }
        
    }
}