using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPool : MonoBehaviour
{
    public static PlatformPool instance;

    public List<Transform> pooledStaticPlatforms;
    public List<Transform> pooledCrumblingPlatforms;

    public Transform staticPlatfrom;
    public Transform crumblingPlatform;
    public Transform platformParent;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        pooledStaticPlatforms = new List<Transform>();
        pooledCrumblingPlatforms = new List<Transform>();

        for(int i = 0; i < 15; i++)
        {
            var tmp = Instantiate(staticPlatfrom, platformParent);
            pooledStaticPlatforms.Add(tmp);
            tmp.gameObject.SetActive(false);
        }
        for (int i = 0; i < 10; i++)
        {
            var tmp = Instantiate(crumblingPlatform, platformParent);
            pooledCrumblingPlatforms.Add(tmp);
            tmp.gameObject.SetActive(false);
        }
    }

    public Transform GetPlatform(PlatformTypes platformType)
    {
        if(platformType == PlatformTypes.StaticPlatform)
        {
            for (int i = 0; i < 15; i++)
            {
                if (!pooledStaticPlatforms[i].gameObject.activeInHierarchy)
                {
                    return pooledStaticPlatforms[i];
                }
            }
            return null;
        }
        else if(platformType == PlatformTypes.StaticPlatform)
        {
            for (int i = 0; i < 10; i++)
            {
                if (!pooledCrumblingPlatforms[i].gameObject.activeInHierarchy)
                {
                    return pooledCrumblingPlatforms[i];
                }
            }
            return null;
        }
        return null;
    }

}
