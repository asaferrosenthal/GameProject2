using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slowtrap : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    //Create a new cube primitive to set the color on
       GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

       //Get the Renderer component from the new cube
       var cubeRenderer = cube.GetComponent<Renderer>();

       //Call SetColor using the shader property name "_Color" and setting the color to red
       cubeRenderer.material.SetColor("_Color", Color.red);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
