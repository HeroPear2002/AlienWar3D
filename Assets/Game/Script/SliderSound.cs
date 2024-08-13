using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderSound : MonoBehaviour
{
    private PlayerAudio pa;

    public Slider volumeSlider;
    private void Start()
    {
        pa = FindObjectOfType<PlayerAudio>();
        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.AddListener(pa.SetSoundVolume);
            volumeSlider.value = pa.Getvolume();
        }
        else
        {
            Debug.LogError("Volume Slider is not assigned in the inspector!");
        }
    }
    public void AdjustVolume(float volume)
    {
        if (pa != null)
        {
            pa.SetSoundVolume(volume);
        }
        else
        {
            Debug.LogError("AudioController is not assigned.");
        }
    }
}
