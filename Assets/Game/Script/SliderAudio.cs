using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderAudio : MonoBehaviour
{
    private AudioController audioController;

    public Slider volumeSlider;
    private void Start()
    {
        audioController = FindObjectOfType<AudioController>();
        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.AddListener(audioController.SetMusicVolume);
            volumeSlider.value = audioController.Getvolume();
        }
        else
        {
            Debug.LogError("Volume Slider is not assigned in the inspector!");
        }
    }
    public void AdjustVolume(float volume)
    {
        if (audioController != null)
        {
            audioController.SetMusicVolume(volume);
        }
        else
        {
            Debug.LogError("AudioController is not assigned.");
        }
    }
}
