using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] Transform levelWall;
    [SerializeField] Camera cameraMain;
    [SerializeField] int startingWalls;

    float wallHeight;
    int nextWallIndex;
    public int NextWallIndex {  get { return nextWallIndex; } set { nextWallIndex = value; } }

    List<Transform> leftWalls = new List<Transform>();
    List<Transform> rightWalls = new List<Transform>();

    bool settingWalls = false;
    void Start()
    {
        wallHeight = levelWall.GetComponent<Transform>().localScale.y;
        //Debug.Log(wallHeight);

        for (int i = 0; i < startingWalls; i++)
        {
            var leftWall = Instantiate(levelWall, transform);
            PlaceWall(leftWall, true);
            leftWalls.Add(leftWall);
            var rightWall = Instantiate(levelWall, transform);
            PlaceWall(rightWall, false);
            rightWalls.Add(rightWall);
        }
    }

    public void PlaceWall(Transform wall, bool isLeftWall)
    {
        string wallName = isLeftWall ? "Left" : "Right";
        float nextPos = wallHeight * nextWallIndex;
        //Debug.Log($"Placing {wallName} Wall at {nextPos}", wall.gameObject);

        wall.GetChild(0).GetComponent<Wall>().IsLeftWall = isLeftWall;
        wall.localPosition = new Vector2(2.5f, nextPos);
        if (isLeftWall)
        {
            wall.localPosition = wall.localPosition * new Vector2(-1, 1);
        }
        else
        {
            nextWallIndex++;
        }
    }

}
