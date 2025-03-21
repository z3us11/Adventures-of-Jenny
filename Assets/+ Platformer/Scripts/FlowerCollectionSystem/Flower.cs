using DG.Tweening;
using Platformer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour
{
    public FlowerColor flowerColor;
    public Color color;
    public ParticleSystem pickupParticle;
    public GameObject flowerLight;

    ParticleSystem subParticle;
    bool collectedFlower = false;

    private void Awake()
    {
        CodeArchitecture.EventManager.StartListening<bool>(CodeArchitecture.EventName.TimeChanged, OnTimeChange);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            if(!collectedFlower)
            {
                collision.transform.parent.GetComponent<PlayerController>().flowerCollection.OnFlowerCollected(flowerColor);
                collision.transform.parent.GetComponent<PlayerController>().staminaConfidence.UpdateStaminaConfidence(10);
                CollectFlower();
                var particle = Instantiate(pickupParticle, transform.position + Vector3.up, Quaternion.identity);

                particle.startColor = color;

                Destroy(particle.gameObject, 0.5f);
                collectedFlower = true;
            }
        }

        if(collision.gameObject.CompareTag("HiddenPath"))
        {
            flowerLight.SetActive(true);
        }
    }

    private void CollectFlower()
    {
        //transform.parent = null;
        AudioManager.instance.PlayCollectFx();

        Sequence tween = DOTween.Sequence();
        tween.Append(transform.DOLocalMoveY(1.75f, 0.5f));
        tween.Append(transform.DOPunchScale(Vector3.one * 0.5f, 0.5f));
        tween.Append(transform.DOScale(0, 0.25f));
        tween.OnComplete(()=>gameObject.SetActive(false));
    }

    void OnTimeChange(bool isNight)
    {
        GetComponent<BoxCollider2D>().enabled = !isNight ? true : false;
        GetComponent<Animator>().enabled = !isNight ? true : false;
    }

    private void OnDestroy()
    {
        CodeArchitecture.EventManager.StopListening<bool>(CodeArchitecture.EventName.TimeChanged, OnTimeChange);
    }
}

public enum FlowerColor
{
    Cyan,
    Magenta,
    Yellow
}
