using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassTile : MonoBehaviour
{
    [SerializeField] Flower[] flowersPrefab;
    [SerializeField] GameObject[] grasses;

    private void Start()
    {
        bool shouldSpawnFlower = Random.Range(0, 1.0f) < 0.5f ? true : false;
        if(shouldSpawnFlower)
        {
            var flower = Instantiate(flowersPrefab[Random.Range(0, flowersPrefab.Length)], transform).gameObject;
            float yPos = Random.Range(0.6f, 0.75f);
            flower.transform.localPosition = new Vector2(Random.Range(-0.25f, 0.25f), yPos);
            int scale = Random.Range(0, 1.0f) < 0.5f ? -1 : 1;
            flower.transform.localScale = new Vector2(scale, 1) * Random.Range(0.75f, 1.15f);
        }

        foreach(var grass in grasses)
        {
            grass.transform.localPosition = new Vector3(grass.transform.localPosition.x, Random.Range(grass.transform.localPosition.y - 0.2f, grass.transform.localPosition.y + 0.1f));
            float scale = Random.Range(0.9f, 1.1f);
            grass.transform.localScale = new Vector3(scale, scale);
        }
        
    }
}
