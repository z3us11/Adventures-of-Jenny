using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PlatformerLevelEditor
{
    public class LevelBlockButton : MonoBehaviour
    {
        public int blockType = -1;
        [HideInInspector]
        public Button btn;
        // Start is called before the first frame update
        void Awake()
        {
            btn = GetComponent<Button>();   
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
