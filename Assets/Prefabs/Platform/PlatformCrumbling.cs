using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformCrumbling : Platform
{
    //*clears throat*
    
    // Start is called before the first frame update
    void Start()
    {
        
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
            if (collision.gameObject.GetComponent<Rigidbody2D>().velocity.y <= 0)
            {
                StartCoroutine(DisablePlatform());
            }
        }
    }

    IEnumerator DisablePlatform()
    {
        yield return new WaitForSeconds(2f);
        Debug.Log("Moving Platform");
        transform.localPosition += new Vector3(15, 0);
        //gameObject.SetActive(false);
    }
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }
}
