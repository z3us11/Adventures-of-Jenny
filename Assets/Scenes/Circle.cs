using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : MonoBehaviour
{
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {

        }
    }


    void FixedUpdate()
    {
        transform.position += new Vector3(0, 0.05f) ;
    }
}
