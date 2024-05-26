using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlatformMoving : Platform
{
    [SerializeField] float xMin, xMax;
    // Start is called before the first frame update
    float moveSpeed = 2f;

    bool isMovingRight = true;

    private void Start()
    {
        //float moveDir = Random.value;
        //if (moveDir <= 0.5f)
        //    MoveLeft();
        //else
        //    MoveRight();
    }

    private void FixedUpdate()
    {
        MovePlatform();
    }

    void MoveRight()
    {
        transform.DOLocalMoveX(xMax, moveSpeed).SetEase(Ease.Linear).OnComplete(()=>MoveLeft());
    }

    void MoveLeft()
    {
        transform.DOLocalMoveX(xMin, moveSpeed).SetEase(Ease.Linear).OnComplete(()=>MoveRight());
    }

    void MovePlatform()
    {
        if (transform.position.x > xMin && !isMovingRight)
            Rb.velocity = new Vector2(-moveSpeed, 0);
        else
            isMovingRight = true;

        if(transform.position.x < xMax && isMovingRight)
            Rb.velocity = new Vector2(moveSpeed, 0);
        else
            isMovingRight = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        if (collision.transform.CompareTag("Player"))
        {
            var playerVelocity = collision.gameObject.GetComponent<Rigidbody2D>().velocity;
            //Add code for combo registering
            //if (playerVelocity.y <= Mathf.Epsilon && playerVelocity.y > -Mathf.Epsilon)
            if(collision.gameObject.GetComponent<PlayerMovement>().IsGrounded())
            {
                playerVelocity = new Vector2(Rb.velocity.x, 0);
                collision.gameObject.GetComponent<Rigidbody2D>().velocity = playerVelocity;
            }
        }
    }
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }
}
