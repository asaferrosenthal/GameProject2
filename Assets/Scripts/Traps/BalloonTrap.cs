using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Traps
{
    public class BalloonTrap : Trap
    {
        public float _MaxScale = 1.5f;
        public float _ScalingFactor = 1.001f;

        // Start is called before the first frame update
        void Start()
        {
            _Enabled = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (_Enabled)
            {
                Debug.Log("GROWING");
                var localScale = transform.localScale;
                localScale = new Vector3(localScale.x+localScale.x*_ScalingFactor*Time.deltaTime, localScale.y+localScale.y*_ScalingFactor * Time.deltaTime, localScale.z+localScale.z*_ScalingFactor * Time.deltaTime);
                transform.localScale = localScale;
                if (localScale.x >= _MaxScale) _Enabled = false;
            }
            else
            {
                Debug.Log("not growing");
            }
            
        }

        protected override void ApplyTrap()
        {
            _Enabled = true;
        }
    }

}
