using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RunMan{
    public class speedtrap : MonoBehaviour
    {
        public MovementControls movementcontrols;
        public GameObject obj;


        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private void OnCollisionEnter(Collision collision){
                //check that player has collided with a trap
                if (collision.gameObject.name == "RunMan"){ 
                    Debug.Log("ENTERED TRAP");
                    
                }
        }

        private void OnCollisionExit(Collision collision)
        {
            Debug.Log("Exited trap");
        }
    }

}
