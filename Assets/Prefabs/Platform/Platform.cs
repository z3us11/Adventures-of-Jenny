using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class Platform : MonoBehaviour
{
    int index;
    public int Index { get { return index; } set { index = value; } }
    PlatformTypes type;
    public PlatformTypes Type { get { return type; } set { type = value; } }

    public static Platform lastPlatform;
    PlatformGenerator platformGeneratorObj;
    private void Start()
    {
        platformGeneratorObj = transform.parent.GetComponent<PlatformGenerator>();
    }
    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            //Add code for combo registering
            if (collision.gameObject.GetComponent<Rigidbody2D>().velocity.y <= 0)
            {
                ScoreManager.instance.UpdateScore(index);
                if (lastPlatform != null)
                {
                    if (lastPlatform.Index < Index - 1)
                    {
                        ScoreManager.instance.ComboRegistering(Index - lastPlatform.Index);
                    }
                    else if (lastPlatform.Index > Index || Index - lastPlatform.Index == 1)
                    {
                        ScoreManager.instance.ComboRegistering(-1);
                    }
                }
                else
                {
                    if(Index > 1)
                        ScoreManager.instance.ComboRegistering(Index);
                }
                lastPlatform = this;
            }
        }
    }
    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("regen"))
        {
            if(platformGeneratorObj == null)
                platformGeneratorObj = transform.parent.GetComponent<PlatformGenerator>();
            platformGeneratorObj.PlacePlatform();
        }
    }

}