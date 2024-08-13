using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    private AudioSource footstepSource;
    [SerializeField]private AudioClip footstepWalkClip;
    [SerializeField]private AudioClip footstepRunClip;
    private AudioSource gunshotSource;
    [SerializeField]private AudioClip pitol; 
    [SerializeField]private AudioClip asset; 
    [SerializeField]private AudioClip SMR; 
    [SerializeField]private AudioClip Shotgun;
    [SerializeField]private AudioClip Sniper;
    [SerializeField]private float Sound = 0.5f;
    [SerializeField]private AudioClip ClickMouse;

    private AudioSource ClickMouseSource;

    private static bool isMusicInitialized = false;

    void Start()
    {
        if (!isMusicInitialized)
        {
            footstepSource = gameObject.AddComponent<AudioSource>();
            footstepSource.loop = true;
            footstepSource.playOnAwake = false;
            footstepSource.volume = Sound;
            gunshotSource = gameObject.AddComponent<AudioSource>();
            gunshotSource.loop = false;
            gunshotSource.playOnAwake = false;
            gunshotSource.volume = Sound;
            ClickMouseSource = gameObject.AddComponent<AudioSource>();
            ClickMouseSource.clip = ClickMouse;
            ClickMouseSource.loop = false;
            ClickMouseSource.playOnAwake = false;
            ClickMouseSource.volume = Sound;
            DontDestroyOnLoad(gameObject);
            isMusicInitialized = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayFootstepWalkSound()
    {
        footstepSource.clip = footstepWalkClip;
    }
    public void PlayFootstepRunSound()
    {
        footstepSource.clip = footstepRunClip;
    }
    public void PlayGunPitol()
    {
        gunshotSource.clip = pitol;
    }
    public void PlayGunAsset()
    {
        gunshotSource.clip = asset;
    }
    public void PlayGunSMR()
    {
        gunshotSource.clip = SMR;
    }
    public void PlayGunShotgun()
    {
        gunshotSource.clip = Shotgun;
    }

    public void PlayGunSniper()
    {
        gunshotSource.clip = Sniper;
    }
    public void PlayGunshotSound()
    {
        if(!gunshotSource.isPlaying)
        {
            gunshotSource.Play();
        }
    }
    public void PlayFootstepSound()
    {
        if (!footstepSource.isPlaying)
        {
            footstepSource.Play();
        }
    }
    public void StopFootstepSound()
    {
        footstepSource.Stop();
    }
    public void StopGunshotSound()
    {
        gunshotSource.Stop();
    }
    public void SetSoundVolume(float volume)
    {
        footstepSource.volume = volume;
        gunshotSource.volume = volume;
        ClickMouseSource.volume = volume;
        Sound = volume;
    }
    public float Getvolume()
    {
        return Sound;
    }
    public void PlayButtonClickSound()
    {
        ClickMouseSource.Play();
    }
}
