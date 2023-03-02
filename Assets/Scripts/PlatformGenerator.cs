using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    [SerializeField] GameObject platformPrefab;

    [SerializeField] float platformStartHeight;
    [SerializeField] float distanceBetweenPlatforms;

    [SerializeField] int startingPlatforms;
    [SerializeField] int checkpointPlatform;

    int nextPlatformIndex;
    public int NextPlatformIndex { get { return nextPlatformIndex; } }

    private void Start()
    {
        for(int i = 0; i < startingPlatforms; i++)
        {
            var platform = Instantiate(platformPrefab, transform);
            PlacePlatform(platform.transform);
        }
    }
    public void PlacePlatform(Transform platform)
    {
        platform.localPosition = new Vector2(0, platformStartHeight + (distanceBetweenPlatforms * nextPlatformIndex));
        if((nextPlatformIndex % checkpointPlatform) - (checkpointPlatform-1) == 0)
        {
            platform.localScale = new Vector2(5, platform.localScale.y);
        }
        nextPlatformIndex++;
        platform.gameObject.name = $"Platform {nextPlatformIndex}";
    }
}
