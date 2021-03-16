using System;
using Adversary;
using Traps;
using RunMan;
using UnityEngine;


namespace Environment {
    public class DeathField : MonoBehaviour
    {
        EnvironmentManager EnvManage = new EnvironmentManager();
        BreakGround BGround = new BreakGround();

        public Renderer rend;
        
        public bool gone; 

        protected void Awake()
        {
            gone = false;
        }

        protected void OnTriggerEnter(Collider other)
        {

            other.gameObject.SetActive(false); //make object disappear

            //check if the RunMan is disabled
            if (other.gameObject.tag == "RunMan"){
                gone = true;
                EnvManage.ResetEnvironment();
                
            }
            
        }
        
        
    }
}

