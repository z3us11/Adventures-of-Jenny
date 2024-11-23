#if UNITY_ANDROID || UNITY_WEBGL

#else
using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamTest : MonoBehaviour
{
    private void Start()
    {
        if (!SteamManager.Initialized)
            return;

        Debug.Log(SteamFriends.GetPersonaName());
    }
}
#endif
