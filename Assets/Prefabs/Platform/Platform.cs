using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    PlatformGenerator parentObj;
    private void Start()
    {
        parentObj = transform.parent.GetComponent<PlatformGenerator>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.CompareTag("Player"))
        {
            //Add code for combo registering
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("regen"))
        {
            parentObj.PlacePlatform(transform);
        }
    }
}
