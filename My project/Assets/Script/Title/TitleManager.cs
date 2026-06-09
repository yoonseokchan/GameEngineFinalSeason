using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class TitleManager : MonoBehaviour
{
    [Header("Scene Management")]
    [SerializeField] private string nextSceneName = "GameScene";

    [Header("UI Panels")]
    [SerializeField] private GameObject settingsPanel; 
    [SerializeField] private GameObject scorePanel;   

    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("Volume Sliders")]
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    [Header("Audio Elements")]
    [SerializeField] private AudioSource sfxAudioSource;
    [SerializeField] private AudioClip buttonClickClip;

    private void Start()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false);

        if (scorePanel != null)
            scorePanel.SetActive(false);

        InitSliders();
    }

    public void StartGame()
    {
        PlaySFX();
        SceneManager.LoadScene(nextSceneName);
    }

    public void OpenSettings()
    {
        PlaySFX();
        if (settingsPanel != null)
            settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        PlaySFX();
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    public void OpenScorePanel()
    {
        PlaySFX();
        if (scorePanel != null)
            scorePanel.SetActive(true);
    }

    public void CloseScorePanel()
    {
        PlaySFX(); 
        if (scorePanel != null)
            scorePanel.SetActive(false);
    }

    public void PlaySFX()
    {
        if (sfxAudioSource != null && buttonClickClip != null)
        {
            sfxAudioSource.PlayOneShot(buttonClickClip);
        }
    }

    public void SetBGMVolume(float value)
    {
        float volume = Mathf.Log10(Mathf.Max(0.0001f, value)) * 20f;
        audioMixer.SetFloat("BGM", volume);
        PlayerPrefs.SetFloat("BGM_Volume", value);
    }

    public void SetSFXVolume(float value)
    {
        float volume = Mathf.Log10(Mathf.Max(0.0001f, value)) * 20f;
        audioMixer.SetFloat("SFX", volume);
        PlayerPrefs.SetFloat("SFX_Volume", value);
    }

    private void InitSliders()
    {
        if (bgmSlider != null)
        {
            bgmSlider.minValue = 0.0001f;
            bgmSlider.maxValue = 1f;
            float savedBGM = PlayerPrefs.GetFloat("BGM_Volume", 1f);
            bgmSlider.value = savedBGM;
            SetBGMVolume(savedBGM);
            bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        }

        if (sfxSlider != null)
        {
            sfxSlider.minValue = 0.0001f;
            sfxSlider.maxValue = 1f;
            float savedSFX = PlayerPrefs.GetFloat("SFX_Volume", 1f);
            sfxSlider.value = savedSFX;
            SetSFXVolume(savedSFX);
            sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        }
    }
}