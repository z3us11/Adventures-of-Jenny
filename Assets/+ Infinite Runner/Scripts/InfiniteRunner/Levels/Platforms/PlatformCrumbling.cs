using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlatformCrumbling : Platform
{
    //*clears throat*
    bool isDisabling = false;
    Tween colorTween = null;

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
            if (collision.gameObject.GetComponent<PlayerMovement>().IsGrounded() && !isDisabling)
            {
                StartCoroutine(DisablePlatform());
            }
        }
    }

    IEnumerator DisablePlatform()
    {
        CrumblingAnimation();

        yield return new WaitForSeconds(0.4f);
        Debug.Log("Moving Platform");

        isDisabling = true;
        transform.localPosition += new Vector3(15, 0);
        //gameObject.SetActive(false);
    }

    void CrumblingAnimation()
    {
        transform.DOShakePosition(0.35f, 0.1f);
        colorTween = transform.GetComponent<SpriteRenderer>().DOColor(new Color(1, 1, 1, 0), 0.75f).SetDelay(0.2f);
        transform.DOMoveY(transform.position.y - 0.2f, 0.2f).SetDelay(0.2f);
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        isDisabling = false;
        if(colorTween != null)
        {
            colorTween.Kill();
            transform.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        }
    }
}
