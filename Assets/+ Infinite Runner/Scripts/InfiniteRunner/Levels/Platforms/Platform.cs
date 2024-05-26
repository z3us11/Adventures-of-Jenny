using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using Unity.VisualScripting;

public abstract class Platform : MonoBehaviour
{
    int index;
    public int Index { get { return index; } set { index = value; } }
    Type type;
    public Type Type { get { return type; } set { type = value; } }

    Rigidbody2D rb;
    public Rigidbody2D Rb { get { return rb; } }

    public static Platform lastPlatform;
    PlatformGenerator platformGeneratorObj;

    bool isMovingDown = false;
    
    private void Start()
    {
        platformGeneratorObj = transform.parent.GetComponent<PlatformGenerator>();
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 0;
    }

    private void Update()
    {
        if(isMovingDown)
        {
            transform.position += Vector3.down * 0.1f;
        }
    }

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            //Add code for combo registering
            var playerMovement = collision.gameObject.GetComponent<PlayerMovement>();
            if (playerMovement.Rb.velocity.y <= 0)
            {
                if (playerMovement.PerfectJump)
                {
                    //ObjectPool.Remove(this);
                    //isMovingDown = true;
                    //rb.gravityScale = 1;
                    //Destroy(transform.GetComponent<Platform>());
                    Debug.Log("changing platform type");
                    //transform.AddComponent<PlatformCrumbling>();
                    //ObjectPool.Add<PlatformCrumbling>(GetComponent<PlatformCrumbling>());
                }

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
                type = lastPlatform.GetType();
            }
        }
    }
    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("regen"))
        {
            if(platformGeneratorObj == null)
                platformGeneratorObj = transform.parent.GetComponent<PlatformGenerator>();
            gameObject.SetActive(false);

            gameObject.TryGetComponent(type, out Component comp);
            ObjectPool.Add((Platform)comp);

            isMovingDown = false;
            platformGeneratorObj.PlacePlatform();
        }
    }

}