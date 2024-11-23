using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotemGem : MonoBehaviour
{
    public Transform totemCavityParent;
    public GameObject totemGlow;

    Tween bobAnim;

    private void Start()
    {
        totemGlow.SetActive(false);

        bobAnim = transform.DOLocalMoveY(transform.localPosition.y + 0.25f, 0.5f).SetEase(Ease.OutCirc).OnComplete(()=>
            {
                transform.DOLocalMoveY(transform.localPosition.y - 0.25f, 0.5f).SetEase(Ease.OutCirc);
            }).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            GetComponent<Collider2D>().enabled = false;
            if(bobAnim != null && bobAnim.IsPlaying()) 
            {
                bobAnim.Kill();
            }
            totemGlow.SetActive(true);
            MoveGemToTotemCavity();
        }
    }

    void MoveGemToTotemCavity()
    {
        transform.DOLocalMoveY(transform.localPosition.y + 0.5f, 1f).SetEase(Ease.InOutQuad).OnComplete(()=>
            {
                transform.SetParent(totemCavityParent);
                transform.DOLocalMove(Vector3.zero, Vector2.Distance(transform.position, totemCavityParent.position) / 5f).SetEase(Ease.InOutQuad).OnComplete(() =>
                    {
                        Debug.Log("Moved");
                    }); ;
            });
    }
}
