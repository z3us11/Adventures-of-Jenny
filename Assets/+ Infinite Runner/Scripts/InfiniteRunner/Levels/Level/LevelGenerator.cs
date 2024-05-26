using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] Transform levelWall;
    [SerializeField] int startingWalls;

    float wallHeight;
    int nextWallIndex;
    public int NextWallIndex {  get { return nextWallIndex; } set { nextWallIndex = value; } }

    List<Transform> walls = new List<Transform>();

    bool settingWalls = false;
    void Start()
    {
        wallHeight = levelWall.GetComponent<Transform>().localScale.y;
        //Debug.Log(wallHeight);

        for (int i = 0; i < startingWalls; i++)
        {
            var wall = Instantiate(levelWall, transform);
            PlaceWall(wall);
            walls.Add(wall);
        }
    }

    public void PlaceWall(Transform wall)
    {
        float nextPos = wallHeight * nextWallIndex;

        //wall.GetChild(0).GetComponent<Wall>().IsLeftWall = isLeftWall;
        wall.localPosition = new Vector2(0, nextPos);

        nextWallIndex++;

        //if (isLeftWall)
        //{
        //    wall.localPosition = wall.localPosition * new Vector2(-1, 1);
        //}
        //else
        //{
        //    nextWallIndex++;
        //}
    }

}
