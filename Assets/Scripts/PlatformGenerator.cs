using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlatformTypes
{
    StaticPlatform,
    CrumblingPlatform
}

public class PlatformGenerator : MonoBehaviour
{
    [SerializeField] GameObject platformPrefab;
    [SerializeField] GameObject crumblingPlatformPrefab;

    [SerializeField] int startingPlatforms;
    [SerializeField] int checkpointPlatform;
    [SerializeField] float checkpointPlatformSize;
    [SerializeField] int crumblingPlatfrom;

    [Space(20)]
    [SerializeField] bool shouldIncreaseDifficulty;

    [Header("Plaform's position and transfrom related variables")]
    [SerializeField] float platformStartHeight;
    [SerializeField] float distanceBetweenPlatforms;
    [SerializeField] float platformStartSize;
    [SerializeField] float platformEndSize;
    [SerializeField] float platformPosX;

    float minDistanceBetweenPlatforms;
    float minPlatformSize;
    float maxPlatformSize;
    float chanceOfCrumblingPlatform = 0.1f;

    int nextPlatformIndex;
    public int NextPlatformIndex { get { return nextPlatformIndex; } }

    private void Start()
    {
        minDistanceBetweenPlatforms = distanceBetweenPlatforms;
        minPlatformSize = platformEndSize - 1.5f;
        maxPlatformSize = platformEndSize;

        for(int i = 0; i < startingPlatforms; i++)
        {
            PlacePlatform();
        }
    }
    public void PlacePlatform()
    {
        float crumblingPlatformProb = UnityEngine.Random.value;
        PlatformTypes type;

        type = (crumblingPlatformProb > chanceOfCrumblingPlatform) ? PlatformTypes.StaticPlatform : PlatformTypes.CrumblingPlatform;
        var platform = PlatformPool.instance.GetPlatform(type);
        Debug.Log(platform.name);
        platform.gameObject.SetActive(true);

        float distance;
        if (UnityEngine.Random.value < 0.65f)
        {
            distance = UnityEngine.Random.Range(minDistanceBetweenPlatforms - minDistanceBetweenPlatforms / 3, minDistanceBetweenPlatforms + minDistanceBetweenPlatforms / 3);
        }
        else
        {
            distance = UnityEngine.Random.Range(minDistanceBetweenPlatforms, distanceBetweenPlatforms);
        }
        platform.localPosition = new Vector2(0, platformStartHeight + distance);

        if(checkpointPlatform != -1 && (nextPlatformIndex % checkpointPlatform) - (checkpointPlatform-1) == 0)
        {
            platform.localScale = new Vector2(checkpointPlatformSize, platform.localScale.y);
        }
        else
        {
            var scale = UnityEngine.Random.Range(minPlatformSize, maxPlatformSize);
            platform.localScale = new Vector2(scale, scale);
            platform.localPosition = new Vector2(UnityEngine.Random.Range(-platformPosX, platformPosX), platform.localPosition.y);
        }
        nextPlatformIndex++;
        platform.gameObject.name = $"{type} {nextPlatformIndex}";

        var platformScript = platform.GetComponent<Platform>();
        if (platformScript != null)
        {
            platformScript.Index = nextPlatformIndex;
            platformScript.Type = type;
        }
        else
        {
            Debug.Log(":/");
        }

        platformStartHeight = platform.position.y;

        if(shouldIncreaseDifficulty)
            ChangePlatforms();
    }

    void ChangePlatforms()
    {
        if(nextPlatformIndex % 50 == 0)
        {
            minPlatformSize = (minPlatformSize < platformStartSize) ? platformStartSize : minPlatformSize - 0.5f;
            maxPlatformSize = (maxPlatformSize < platformStartSize) ? platformStartSize : maxPlatformSize - 0.25f;
            platformPosX = (platformPosX >= 4) ? 4 : platformPosX + 0.25f;
            distanceBetweenPlatforms = (distanceBetweenPlatforms >= 5) ? 5 : distanceBetweenPlatforms + 0.25f;
            chanceOfCrumblingPlatform += 0.05f;
        }
    }
}
