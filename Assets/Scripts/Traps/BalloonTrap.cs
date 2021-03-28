using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonTrap : MonoBehaviour
{
    public float timerate = 0.5f; //lower value will make it inflate and deflate more slowly
    public int growthfactor = 12; //greater value will make it inflate and deflate more drastically
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
                
        //uses sign function to increase and decrease its scale overtime
        Vector3 vec = new Vector3(growthfactor * Mathf.Sin(Time.time * timerate), growthfactor * Mathf.Sin(Time.time*timerate), growthfactor * Mathf.Sin(Time.time*timerate));
        transform.localScale = vec;
    }
}
