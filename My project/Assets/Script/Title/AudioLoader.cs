using UnityEngine;
using UnityEngine.Audio;

public class AudioVolumeLoader : MonoBehaviour
{
    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer audioMixer; 

    private void Start()
    {
        LoadAndApplyVolume("BGM", "BGM_Volume");
        LoadAndApplyVolume("SFX", "SFX_Volume");
    }

    private void LoadAndApplyVolume(string mixerParameter, string prefsKey)
    {
        if (audioMixer == null) return;

        float savedValue = PlayerPrefs.GetFloat(prefsKey, 1f);

        float volume = Mathf.Log10(Mathf.Max(0.0001f, savedValue)) * 20f;
        audioMixer.SetFloat(mixerParameter, volume);
    }
}