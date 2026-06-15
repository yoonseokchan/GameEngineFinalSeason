using UnityEngine;
using TMPro; // 점수를 화면에 그리기 위한 TextMeshPro UI 네임스페이스

public class PlayerExpManager : MonoBehaviour
{
    public static PlayerExpManager Instance { get; private set; }

    [Header("레벨업 UI 패널")]
    [SerializeField] private GameObject levelUpPanel;

    [Header("인게임 UI 텍스트 연결")]
    [Tooltip("현재 획득한 점수를 실시간으로 띄워줄 TextMeshPro - Text 오브젝트를 연결하세요.")]
    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("경험치 및 점수 시스템")]
    [SerializeField] private int currentExp = 0;
    [SerializeField] private int maxExp = 100;
    [SerializeField] private int currentLevel = 1;

    // 현재 한 판당 실시간으로 올라가는 점수
    private int currentScore = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        if (levelUpPanel != null) levelUpPanel.SetActive(false);

        // 게임 시작 시 점수 초기화 및 UI 텍스트 업데이트
        currentScore = 0;
        UpdateScoreUI();

        // 참고: 새로 시작할 때 이전 판 점수를 날리기 위해 초기화합니다.
        // 최고 기록 저장은 아래 SaveCurrentScoreToTotal() 에서 처리합니다.
        PlayerPrefs.SetInt("LastMatchScore", 0);
        PlayerPrefs.Save();
    }

    public void AddExperience(int expAmount, int scoreAmount)
    {
        currentExp += expAmount;
        if (currentExp >= maxExp)
        {
            LevelUp();
        }

        currentScore += scoreAmount;
        UpdateScoreUI();

        PlayerPrefs.SetInt("LastMatchScore", currentScore);


        int bestScore = PlayerPrefs.GetInt("BestScore", 0);
        if (currentScore > bestScore)
        {
            PlayerPrefs.SetInt("BestScore", currentScore);
        }

        PlayerPrefs.Save(); 
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score : {currentScore}";
        }
    }

    private void LevelUp()
    {
        currentLevel++;
        currentExp -= maxExp;
        maxExp = Mathf.RoundToInt(maxExp * 1.2f);

        if (levelUpPanel != null)
        {
            levelUpPanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void CloseLevelUpPanel()
    {
        if (levelUpPanel != null)
        {
            levelUpPanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}