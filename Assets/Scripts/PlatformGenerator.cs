using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    [SerializeField] GameObject platformPrefab;

    [SerializeField] int startingPlatforms;
    [SerializeField] int checkpointPlatform;

    [SerializeField] bool shouldIncreaseDifficulty;

    [Header("Plaform's position and transfrom related variables")]
    [SerializeField] float platformStartHeight;
    [SerializeField] float distanceBetweenPlatforms;
    [SerializeField] float minPlatformSize;
    [SerializeField] float maxPlatformSize;
    [SerializeField] float platformPosX;
    [SerializeField] float checkpointPlatformSize;

    float minDistanceBetweenPlatforms;
    int nextPlatformIndex;
    public int NextPlatformIndex { get { return nextPlatformIndex; } }

    private void Start()
    {
        minDistanceBetweenPlatforms = distanceBetweenPlatforms;

        for(int i = 0; i < startingPlatforms; i++)
        {
            var platform = Instantiate(platformPrefab, transform);
            PlacePlatform(platform.transform);
        }
    }
    public void PlacePlatform(Transform platform)
    {
        float distance;

        if (Random.value < 0.5f)
            distance = Random.Range(minDistanceBetweenPlatforms, distanceBetweenPlatforms - distanceBetweenPlatforms / 4);
        else
            distance = Random.Range(minDistanceBetweenPlatforms, distanceBetweenPlatforms);

        platform.localPosition = new Vector2(0, platformStartHeight + distance);
        if((nextPlatformIndex % checkpointPlatform) - (checkpointPlatform-1) == 0)
        {
            platform.localScale = new Vector2(checkpointPlatformSize, platform.localScale.y);
        }
        else
        {
            platform.localScale = new Vector2(Random.Range(minPlatformSize, maxPlatformSize), platform.localScale.y);
            platform.localPosition = new Vector2(Random.Range(-platformPosX, platformPosX), platform.localPosition.y);
        }
        nextPlatformIndex++;
        platform.gameObject.name = $"Platform {nextPlatformIndex}";
        platform.GetComponent<Platform>().Index = nextPlatformIndex;

        platformStartHeight = platform.position.y;

        if(shouldIncreaseDifficulty)
            ChangePlatforms();
    }

    void ChangePlatforms()
    {
        if(nextPlatformIndex % 50 == 0)
        {
            minPlatformSize = (minPlatformSize <= 1) ? 1f : minPlatformSize - 0.25f;
            maxPlatformSize = (maxPlatformSize <= 1.5) ? 1.5f : maxPlatformSize - 0.25f;
            platformPosX = (platformPosX >= 4) ? 4 : platformPosX + 0.25f;
            distanceBetweenPlatforms = (distanceBetweenPlatforms >= 5) ? 5 : distanceBetweenPlatforms + 0.25f;
        }
    }
}
