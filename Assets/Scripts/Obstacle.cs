using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{

    [SerializeField] Sprite[] obstacleSprites;

    bool canGiveDamage = true;


    SpriteRenderer obstacleImg;
    

    // Start is called before the first frame update
    void Start()
    {
        obstacleImg = transform.GetChild(0).GetComponent<SpriteRenderer>(); 
        ChangeObstacleSprite();
    }

    private void ChangeObstacleSprite()
    {
        //if (obstacleSprites == null || obstacleSprites.Length == 0)
        //    return;
        if (Random.value > 0.5f)
        {
            //obstacleImg.sprite = obstacleSprites[1];
            obstacleImg.color = Color.red;
            canGiveDamage = true;
        }
        else
        {
            //obstacleImg.sprite = obstacleSprites[0];
            obstacleImg.color = Color.cyan;
            canGiveDamage = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player Collided");
            var player = collision.gameObject.GetComponent<PlayerMovement>();
            if(player.GetComponent<Rigidbody2D>().velocity.y <= 0)
            {
                player.SteppingOnObstacle(canGiveDamage);
                if(!canGiveDamage)
                    StartCoroutine(DestroyObstacle());            
            }
        }
    }

    IEnumerator DestroyObstacle()
    {
        yield return new WaitForSeconds(0.5f);

        gameObject.SetActive(false);
    }
}
