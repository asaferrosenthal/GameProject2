using System;
using Environment;
using UnityEngine;

namespace UI
{
    public class LevelCountDownUI : LevelUIBase
    {
        [Tooltip("The animation that will be played on level start.")]
        public Animator _CountDown;

        public Animation _Animation;

        private void OnEnable()
        {
            _CountDown.Play("Base Layer.CountDown", 0, 0f);
            _Manager?.InitializeLevelStart();
        }

        private void OnDisable()
        {
            _Manager.LevelStart();
        }
    }
}
