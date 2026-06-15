using UnityEngine;
using UnityEngine.UI; 

public class InGameUIManager : MonoBehaviour
{
    public static InGameUIManager Instance { get; private set; }

    [Header("게임오버 UI 설정 (하이러키 내 오브젝트)")]
    [SerializeField] private GameObject gameOverPanel;

    [Header("체력 시각화 UI")]
    [Tooltip("UI의 Slider 컴포넌트를 연결하세요.")]
    [SerializeField] private Slider hpSlider;

    [Tooltip("피격 시 화면이 순간 붉게 변할 UI Image 패널을 연결하세요.")]
    [SerializeField] private Image damageFlashImage;
    [SerializeField] private float flashSpeed = 5f;
    private bool isFlashing = false;

    [Header("사운드 설정")]
    [SerializeField] private AudioSource bgmAudioSource; 
    [SerializeField] private AudioClip gameOverBgm;     

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (damageFlashImage != null) damageFlashImage.color = Color.clear;
    }

    private void Update()
    {
        if (isFlashing && damageFlashImage != null)
        {
            damageFlashImage.color = Color.Lerp(damageFlashImage.color, Color.clear, flashSpeed * Time.deltaTime);
            if (damageFlashImage.color.a <= 0.05f)
            {
                damageFlashImage.color = Color.clear;
                isFlashing = false;
            }
        }
    }

    public void InitializeHPBar(int maxHp)
    {
        if (hpSlider != null)
        {
            hpSlider.maxValue = maxHp;
            hpSlider.value = maxHp;
        }
    }

    public void UpdateHPBar(int currentHp)
    {
        if (hpSlider != null)
        {
            hpSlider.value = currentHp;
        }

        if (damageFlashImage != null && currentHp > 0)
        {
            damageFlashImage.color = new Color(1f, 0f, 0f, 0.4f);
            isFlashing = true;
        }
    }

    public void TriggerGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        // 2. BGM 전환 로직
        if (bgmAudioSource != null && gameOverBgm != null)
        {
            bgmAudioSource.Stop();              
            bgmAudioSource.clip = gameOverBgm; 
            bgmAudioSource.loop = false;       
            bgmAudioSource.Play();            
        }

        // 3. 게임 플레이 정지
        Time.timeScale = 0f;
        Debug.Log("게임 오버! 패널 오픈 및 BGM이 전환되었습니다.");
    }
}