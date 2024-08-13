using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class AudioController : MonoBehaviour
{
    public AudioClip backgroundMusic1;
    public AudioClip backgroundMusic2;

    private AudioSource audioSource;

    private int changescene0 = 0;
    private int changescene1 = 0;
    private static bool isMusicInitialized = false;

    private void Start()
    {
        if (!isMusicInitialized)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.loop = true;
            audioSource.playOnAwake = false;
            audioSource.volume = 0.5f;
            DontDestroyOnLoad(gameObject);
            isMusicInitialized = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.buildIndex == 0 && changescene0 == 0)
        {
            changescene0 = 1;
            changescene1 = 0;
            audioSource.clip = backgroundMusic1;
            audioSource.Play();
        }
        else if (currentScene.buildIndex == 1 && changescene1 == 0)
        {
            changescene1 = 1;
            changescene0 = 0;
            audioSource.clip = backgroundMusic2;
            audioSource.Play();
        }
    }
    public void SetMusicVolume(float volume)
    {
        if (audioSource != null)
        {
            audioSource.volume = volume;
        }
        else
        {
            Debug.LogError("AudioSource is not assigned!");
        }
    }
    public float Getvolume()
    {
        return audioSource.volume;
    }
}
