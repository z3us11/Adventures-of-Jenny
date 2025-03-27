using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class AudioManager : MonoBehaviour
{
    [Header("Music")]
    public AudioSource music;
    [Header("Sound Fxs")]
    public AudioSource walkSFxAudioSource;
    public AudioSource grassSFxAudioSource;
    public AudioSource collectSFxAudioSource;
    public AudioSource abilityCollectSFxAudioSource;
    public AudioSource doorOpenAudioSource;
    public AudioSource jumpSFxAudioSource;
    public AudioSource perfectJumpSFxAudioSource;
    [Space]
    public AudioClip[] collectSoundFxs;
    public AudioClip[] perfectJumpSoundFxs;
    public SoundFxAudio[] soundFxs;
    [Header("Ambient Sounds")]
    public AudioSource dayAmbientSound;
    public AudioSource nightAmbientSound;
    public AudioSource windAmbientSound;
    public AudioSource caveAmbientSound;

    bool canPlaySounds;
    bool canPlayMusic;

    List<AudioSource> collectAudioSrcs = new List<AudioSource>();

    public static AudioManager instance;
    public enum SoundFxs
    {
        None,
        Jump,
        PerfectJump,
        GrassWalk,
        GrassSlide
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        walkSFxAudioSource.volume = 0;
        grassSFxAudioSource.volume = 0;

        for (int i = 0; i < 5; i++)
        {
            AudioSource newSource = gameObject.AddComponent<AudioSource>();
            collectAudioSrcs.Add(newSource);
        }
    }

    public void CanPlaySounds(bool value)
    {
        canPlaySounds = value;
    }

    public void CanPlayMusic(bool value)
    {
        canPlayMusic = value;
        if (canPlayMusic)
        {
            music.enabled = true;
            dayAmbientSound.enabled = true;
            nightAmbientSound.enabled = true;
            windAmbientSound.enabled = true;
            caveAmbientSound.enabled = true;
        }
        else
        {
            music.enabled = false;
            dayAmbientSound.enabled = false;
            nightAmbientSound.enabled = false;
            windAmbientSound.enabled = false;
            caveAmbientSound.enabled = false;
        }
    }


    public void PlayDoorOpenSound()
    {
        if (!canPlaySounds)
            return;

        doorOpenAudioSource.Play();
    }

    public void PlayAbilityCollectFx()
    {
        if (!canPlaySounds)
            return;

        abilityCollectSFxAudioSource.Play();
    }

    public void PlayCollectFx()
    {
        if (!canPlaySounds)
            return;

        foreach (AudioSource source in collectAudioSrcs)
        {
            if (!source.isPlaying)
            {
                source.clip = collectSoundFxs[UnityEngine.Random.Range(0, collectSoundFxs.Length)];
                source.volume = collectSFxAudioSource.volume;
                //source.pitch = UnityEngine.Random.Range(0.75f, 1.25f);
                source.Play();
                return;
            }
        }

        // If all AudioSources are busy, create a new one
        AudioSource newSource = gameObject.AddComponent<AudioSource>();
        newSource.clip = collectSoundFxs[UnityEngine.Random.Range(0, collectSoundFxs.Length)];
        newSource.volume = collectSFxAudioSource.volume;
        newSource.Play();
        collectAudioSrcs.Add(newSource);
    }

    public void PlaySoundFx(SoundFxs soundFx)
    {
        if (!canPlaySounds)
            return;

        if (soundFx == SoundFxs.Jump)
        {
            jumpSFxAudioSource.clip = soundFxs[0].sFxAudioClip;
            jumpSFxAudioSource.Play();
            return;
        }

        //if (soundFx == SoundFxs.PerfectJump)
        //{
        //    perfectJumpSFxAudioSource.clip = perfectJumpSoundFxs[0];
        //    perfectJumpSFxAudioSource.Play();
        //    return;
        //}

        for (int i = 0; i < soundFxs.Length; i++)
        {
            if (soundFx == soundFxs[i].sFxType)
            {
                walkSFxAudioSource.pitch = 1f;
                walkSFxAudioSource.clip = soundFxs[i].sFxAudioClip;
                walkSFxAudioSource.Play();
                break;
            }
        }
    }

    public void PlaySoundFx(SoundFxs soundFx, float pitch)
    {
        if (!canPlaySounds)
            return;

        if (soundFx == SoundFxs.GrassWalk)
        {
            walkSFxAudioSource.pitch = 1.5f;
            walkSFxAudioSource.clip = soundFxs[1].sFxAudioClip;
            walkSFxAudioSource.Play();
            return;
        }
    }

    public void SwitchDay(bool isNight, float time)
    {
        if (!canPlayMusic)
            return;

        if (!isNight)
        {
            dayAmbientSound.DOFade(0.3f, time);
            nightAmbientSound.DOFade(0, time);
            music.DOFade(0.1f, time);
        }
        else
        {
            dayAmbientSound.DOFade(0, time);
            nightAmbientSound.DOFade(0.3f, time);
            music.DOFade(0.02f, time);
        }
    }

    [Serializable]
    public class SoundFxAudio
    {
        public SoundFxs sFxType;
        public AudioClip sFxAudioClip;
    }

}

