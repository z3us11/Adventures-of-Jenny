using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource sFxAudioSource;

    public SoundFxAudio[] soundFxs;


    public static AudioManager instance;
    public enum SoundFxs
    {
        None,
        CoinAdd
    }

    private void Awake()
    {
        instance = this;
    }


    public void PlaySoundFx(SoundFxs soundFx)
    {
        for(int i = 0; i < soundFxs.Length; i++)
        {
            if (soundFx == soundFxs[i].sFxType)
            {
                sFxAudioSource.clip = soundFxs[i].sFxAudioClip;
                sFxAudioSource.Play();
            }


        }
    }
    [Serializable]
    public class SoundFxAudio
    {
        public SoundFxs sFxType;
        public AudioClip sFxAudioClip;
    }

}

