using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;




namespace PlatformerLevelEditor
{
    public class LevelEditor : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] LevelBlockButton[] levelBlockButtons;
        [SerializeField] Button clearButton;
        [Space]
        [SerializeField] Button saveButton;
        [SerializeField] Button loadButton;

        [Header("Level")]
        [SerializeField] Transform levelLayout;
        [SerializeField]
        LevelBlockUI levelBlockUIGameObject;
        public int width;
        public int height;

        LevelBlockUI[,] allLevelBlocks;
        [HideInInspector]
        public LevelBlockButton selectedLevelBlock;

        public static LevelEditor instance;

        private void Awake()
        {
            instance = this;
        }

        // Start is called before the first frame update
        void Start()
        {
            allLevelBlocks = new LevelBlockUI[width, height];

            int i = 0;
            foreach(var btn  in levelBlockButtons)
            {
                int index = i++;
                btn.btn.onClick.RemoveAllListeners();
                btn.btn.onClick.AddListener(() => OnClickLevelBlockButtons(index));
            }
            clearButton.onClick.RemoveAllListeners();
            clearButton.onClick.AddListener(() => OnClickLevelBlockButtons(-1));

            saveButton.onClick.RemoveAllListeners();
            loadButton.onClick.RemoveAllListeners();
            saveButton.onClick.AddListener(() => SaveData());
            loadButton.onClick.AddListener(() => LoadData());

            GenerateLevelBlocks();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SaveData()
        {
            LevelBlockData[,] levelBlockData = new LevelBlockData[width, height];
            for (int i = 0; i < allLevelBlocks.GetLength(0); i++)
            {
                for (int j = 0; j < allLevelBlocks.GetLength(1); j++)
                {
                    levelBlockData[i, j] = allLevelBlocks[i, j].GetData();
                }
            }
            string saveData = JsonConvert.SerializeObject(levelBlockData);
            PlayerPrefs.SetString("SaveData", saveData);
            Debug.Log($"Saving Data: {saveData}");
        }

        public void LoadData()
        {
            string saveData = PlayerPrefs.GetString("SaveData", "");
            Debug.Log($"Loading Data: {saveData}");

            LevelBlockData[,] levelBlockData = new LevelBlockData[width, height];
            levelBlockData = JsonConvert.DeserializeObject<LevelBlockData[,]>(saveData);

            GenerateLevelBlocks(levelBlockData);
        }

        void GenerateLevelBlocks(LevelBlockData[,] levelBlockData = null)
        {
            for(int i = 0; i < levelLayout.childCount; i++)
            {
                Destroy(levelLayout.GetChild(i).gameObject);
            }
            allLevelBlocks = new LevelBlockUI[width, height];   

            if(levelBlockData == null)
            {
                for (int i = 0; i < allLevelBlocks.GetLength(0); i++)
                {
                    for (int j = 0; j < allLevelBlocks.GetLength(1); j++)
                    {
                        var levelBlockUI = Instantiate(levelBlockUIGameObject, levelLayout);
                        allLevelBlocks[i, j] = levelBlockUI;
                        levelBlockUI.SetData(i, j);
                    }
                }
            }
            else
            {
                for (int i = 0; i < allLevelBlocks.GetLength(0); i++)
                {
                    for (int j = 0; j < allLevelBlocks.GetLength(1); j++)
                    {
                        var levelBlockUI = Instantiate(levelBlockUIGameObject, levelLayout);
                        allLevelBlocks[i, j] = levelBlockUI;
                        levelBlockUI.SetData(levelBlockData[i, j].xCoordinate, levelBlockData[i, j].yCoordinate, levelBlockData[i, j].blockType);
                    }
                }
            }
            
        }

        void OnClickLevelBlockButtons(int index = -1)
        {
            foreach (var button in levelBlockButtons)
            {
                button.btn.interactable = true;
            }
            if (index == -1)
            {
                selectedLevelBlock = null;
                return;
            }

            levelBlockButtons[index].btn.interactable = false;
            selectedLevelBlock = levelBlockButtons[index];
        }
    }
}

