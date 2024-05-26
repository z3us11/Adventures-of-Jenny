using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformerLevelEditor
{
    public class LevelGenerator : MonoBehaviour
    {
        [SerializeField] GameObject[] levelBlock;
        [SerializeField] int xLimit = 19;
        [SerializeField] int yLimit = 10;

        List<GameObject> levelBoundary = new List<GameObject>();
        GameObject[,] allLevelBlocks;

        // Start is called before the first frame update
        void Start()
        {

            SpawnLevel();
        }

        // Update is called once per frame
        void Update()
        {

        }

        void SpawnLevel()
        {
            SpawnLevelBoundary();
            //SpawnRestLevel();
        }

        public void SpawnRestLevel()
        {
            string saveData = PlayerPrefs.GetString("SaveData", "");
            LevelBlockData[,] levelBlockData = JsonConvert.DeserializeObject<LevelBlockData[,]>(saveData);

            allLevelBlocks = new GameObject[LevelEditor.instance.width, LevelEditor.instance.height];
            for (int i = 0; i < allLevelBlocks.GetLength(0); i++)
            {
                for(int j = 0; j < allLevelBlocks.GetLength(1); j++)
                {
                    //0, 0 -> (-19), (9-1)

                    if (levelBlockData[i,j].blockType != -1)
                    {
                        GameObject _levelBlock = Instantiate(levelBlock[levelBlockData[i, j].blockType], transform);
                        int xCoordinate = 1 + levelBlockData[i, j].xCoordinate;
                        int yCoordinate = 1 + levelBlockData[i, j].yCoordinate;
                        Vector2 blockPos = new Vector2(xCoordinate, yCoordinate);

                        _levelBlock.name = $"LevelBlock_{xCoordinate}_{yCoordinate}";
                        _levelBlock.transform.localPosition = blockPos;
                    }
                }
            }
        }

        void SpawnLevelBoundary()
        {
            for (int x = 0; x <= xLimit; x++)
            {
                Vector2 blockPos = new Vector2(x, 0);
                var _levelBlockBelow = Instantiate(levelBlock[0], blockPos, Quaternion.identity);
                _levelBlockBelow.gameObject.name = $"LevelBlock_{x}_{0}";
                _levelBlockBelow.transform.parent = transform.GetChild(0);
                levelBoundary.Add(_levelBlockBelow);

                blockPos = new Vector2(x, yLimit);
                var _levelBlockUp = Instantiate(levelBlock[0], blockPos, Quaternion.identity);
                _levelBlockUp.gameObject.name = $"LevelBlock_{x}_{yLimit}";
                _levelBlockUp.transform.parent = transform.GetChild(0);
                levelBoundary.Add(_levelBlockBelow);
            }

            for (int y = 0; y <= yLimit; y++)
            {
                Vector2 blockPos = new Vector2(0, y);
                var _levelBlockLeft = Instantiate(levelBlock[0], blockPos, Quaternion.identity);
                _levelBlockLeft.gameObject.name = $"LevelBlock_{0} _ {y}";
                _levelBlockLeft.transform.parent = transform.GetChild(0); ;
                levelBoundary.Add(_levelBlockLeft);

                blockPos = new Vector2(xLimit, y);
                var _levelBlockRight = Instantiate(levelBlock[0], blockPos, Quaternion.identity);
                _levelBlockRight.gameObject.name = $"LevelBlock_{xLimit}_{y}";
                _levelBlockRight.transform.parent = transform.GetChild(0);
                levelBoundary.Add(_levelBlockLeft);
            }
        }
    }

}
