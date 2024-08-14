using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stars : MonoBehaviour
{
    public GameObject[] stars;
    public int maxStars;

    public float xRange;
    public float yRange;

    [Space]
    public Material[] starsMaterial;
    public float glowDuration = 0.25f;
    float glowAmount;
    int glowAmountShader = Shader.PropertyToID("_glowEffectAmount");

    private void Start()
    {
        for(int j = 0; j < 4; j++)
        {
            GameObject starGroup = new GameObject();
            starGroup.name = "StarGroup_" + j;
            starGroup.transform.parent = transform;
            starGroup.transform.localPosition = Vector3.zero;
            for (int i = 0; i < maxStars; i++)
            {
                GameObject starObj = Instantiate(stars[j], starGroup.transform);
                starObj.transform.localPosition = new Vector3(Random.Range(-xRange, xRange), Random.Range(-yRange, yRange), 10);
                starObj.transform.localScale = Vector3.one * Random.Range(0.5f, 1f);
                starObj.GetComponent<Star>().GlowStart(starObj.GetComponent<SpriteRenderer>().material);

                //starObj.GetComponent<SpriteRenderer>().material = starsMaterial[j];
                //starsMaterial[j].SetFloat("_glowEffectAmount", Random.Range(5f, 25f));
                //Debug.Log(starObj.GetComponent<SpriteRenderer>().material.GetFloat("_glowEffectAmount"));
            }
        }
        
    }

    

}
