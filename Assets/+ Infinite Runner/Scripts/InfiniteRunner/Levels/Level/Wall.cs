using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    LevelGenerator parentObj;
    private void Start()
    {
        parentObj = transform.parent.parent.GetComponent<LevelGenerator>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("regen"))
        {
            parentObj.PlaceWall(transform.parent);
        }
    }

    
}
