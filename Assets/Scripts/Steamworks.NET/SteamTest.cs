
#if !UNITY_ANDROID && !UNITY_WEBGL
using Steamworks;
#endif

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamTest : MonoBehaviour
{

    private void Start()
    {
#if !UNITY_ANDROID && !UNITY_WEBGL

        if (!SteamManager.Initialized)
            return;

        Debug.Log(SteamFriends.GetPersonaName());
#endif

    }
}
