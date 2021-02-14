using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Traps
{
    public class BalloonTrap : Trap
    {
        //public float scalingFactor = 5.0f;
        protected override void ApplyTrap()
        {
            if (!_Enabled) return;
            Debug.Log("BALLOOOON");
            GameObject ball = GameObject.FindWithTag("Balloon");
            //ball.transform.localScale = new Vector3(transform.localScale.x+transform.localScale.x*scalingFactor*Time.deltaTime, transform.localScale.y+transform.localScale.y*scalingFactor * Time.deltaTime, transform.localScale.z+transform.localScale.z*scalingFactor * Time.deltaTime);
        }
    }
}

/*
public class BalloonTrap : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
*/