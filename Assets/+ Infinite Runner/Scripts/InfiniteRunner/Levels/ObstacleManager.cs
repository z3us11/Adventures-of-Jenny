using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    [SerializeField] Obstacle obstaclePrefab;

    public static ObstacleManager instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //SpawnObstacles();
    }

    public void SpawnObstacles()
    {
        for (int i = 0; i < 20; i++)
        {
            var obstacle = Instantiate(obstaclePrefab, transform);
            //obstacle.gameObject.SetActive(false);
            ObjectPool.Add<Obstacle>(obstacle);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaceObstacle(Transform parentPlatform, float posX)
    {
        if (Random.value < 0.75f)
            return;

        var obstacle = ObjectPool.Get<Obstacle>();
        Debug.LogError(ObjectPool.GetActiveInPool<Obstacle>());
        Debug.LogError(ObjectPool.GetAmountInPool<Obstacle>());
        obstacle.gameObject.SetActive(true);
        obstacle.transform.SetParent(parentPlatform);
        obstacle.transform.localPosition = Vector3.zero;
        var pos = obstacle.transform.localPosition;
        obstacle.transform.localPosition = new Vector3(posX, pos.y + 0.17f, pos.z);
    }
}
