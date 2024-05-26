using PlatformerLevelEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PlatformerLevelEditor
{
    public class LevelBlockUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerClickHandler
    {
        Image blockImg;
        [SerializeField]
        Color[] blockColors;
        Color noColor = new Color(1f, 1f, 1f, 0);
        Color previousColor;
        LevelBlockData levelBlockData = new LevelBlockData();

        // Start is called before the first frame update
        void Start()
        {
            blockImg = GetComponent<Image>();
        }

        // Update is called once per frame
        void Update()
        {
            //if (levelBlockData.xCoordinate == 0 && levelBlockData.yCoordinate == 0)
            //    Debug.Log(blockImg.color);
        }

        public void SetData(int x, int y, int blockType = -1)
        {
            if (blockImg == null)
            {
                blockImg = GetComponent<Image>();
            }

            gameObject.name = $"LevelBlockUI_{x}_{y}";
            levelBlockData.xCoordinate = x;
            levelBlockData.yCoordinate = y;
            levelBlockData.blockType = blockType;
            if (blockType == -1)
                blockImg.color = noColor;
            else
            {
                blockImg.color = blockColors[blockType];
                
                //Color color = HexToColor(blockTYpe);
                //blockImg.color = color;
            }
            previousColor = blockImg.color;
        }

        public LevelBlockData GetData()
        {
            return levelBlockData;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            ChangeLevelBlock();
        }

        private void ChangeLevelBlock()
        {
            if (LevelEditor.instance.selectedLevelBlock == null)
            {
                blockImg.color = noColor;
                levelBlockData.blockType = -1;
            }
            else
            {
                blockImg.color = LevelEditor.instance.selectedLevelBlock.btn.colors.normalColor;
                levelBlockData.blockType = LevelEditor.instance.selectedLevelBlock.blockType;
            }
            previousColor = blockImg.color;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (LevelEditor.instance.selectedLevelBlock == null)
            {
                blockImg.color = noColor;
            }
            else
            {
                blockImg.color = LevelEditor.instance.selectedLevelBlock.btn.colors.normalColor;
            }
            if (Input.GetMouseButton(0))
            {
                ChangeLevelBlock();
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            blockImg.color = previousColor;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            ChangeLevelBlock();

        }

        public Color HexToColor(string hex)
        {
            // Remove any leading "#" characters
            hex = hex.TrimStart('#');

            // Convert hex string to Color
            Color color = new Color();
            ColorUtility.TryParseHtmlString("#" + hex, out color);

            return color;
        }

        public string ColorToHex(Color color)
        {
            // Convert color components to hexadecimal
            int r = (int)(color.r * 255f);
            int g = (int)(color.g * 255f);
            int b = (int)(color.b * 255f);

            // Format as hexadecimal string
            string hex = "#" + string.Format("{0:X2}{1:X2}{2:X2}", r, g, b);

            return hex;
        }
    }

    [Serializable]
    public class LevelBlockData
    {
        public int blockType = -1;
        public int xCoordinate;
        public int yCoordinate;
    }
}

