//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class NewPlatform : MonoBehaviour
//{
//    int index;
//    public int Index { get { return index; } set { index = value; } }

//    PlatformGenerator platformGeneratorObj;

//    // Start is called before the first frame update
//    void Start()
//    {
        
//    }

//    // Update is called once per frame
//    void Update()
//    {
        
//    }

//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        if (collision.transform.CompareTag("regen"))
//        {
//            if (platformGeneratorObj == null)
//                platformGeneratorObj = transform.parent.GetComponent<PlatformGenerator>();
//            gameObject.SetActive(false);

//            gameObject.TryGetComponent(type, out Component comp);
//            ObjectPool.Add(comp);

//            platformGeneratorObj.PlacePlatform();
//        }
//    }
//}
