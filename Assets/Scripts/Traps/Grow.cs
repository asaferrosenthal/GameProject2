using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Traps{
    public class Grow : Trap
    {
        public float scalingFactor = 1.001f;

        // Start is called before the first frame update
        void Start()
        {
            _Enabled = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (_Enabled){
                Debug.Log("GROWING");
                transform.localScale = new Vector3(transform.localScale.x+transform.localScale.x*scalingFactor*Time.deltaTime, transform.localScale.y+transform.localScale.y*scalingFactor * Time.deltaTime, transform.localScale.z+transform.localScale.z*scalingFactor * Time.deltaTime);
            }
            else{
                Debug.Log("not growing");
            }
            
        }

        
    }

}
