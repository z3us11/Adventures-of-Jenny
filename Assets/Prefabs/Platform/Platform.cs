using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Platform : MonoBehaviour
{
    int index;
    public int Index { get { return index; } set { index = value; } }

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
            if(collision.gameObject.GetComponent<Rigidbody2D>().velocity.y <= 0)
                ScoreManager.instance.UpdateScore(index);
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
