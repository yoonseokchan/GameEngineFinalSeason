using UnityEngine;

public class PlayerExpManager : MonoBehaviour
{

    public static PlayerExpManager Instance { get; private set; }

    [Header("레벨업 UI 패널 (하이러키에서 연결)")]
    [SerializeField] private GameObject levelUpPanel;

    [Header("경험치 시스템 설정")]
    [SerializeField] private int currentExp = 0;
    [SerializeField] private int maxExp = 100; 
    [SerializeField] private int currentLevel = 1;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        if (levelUpPanel != null)
        {
            levelUpPanel.SetActive(false);
        }
    }

    public void AddExperience(int amount)
    {
        currentExp += amount;
        Debug.Log($"경험치 획득: +{amount} (현재: {currentExp}/{maxExp})");

        if (currentExp >= maxExp)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        currentLevel++;
        currentExp -= maxExp; 
        maxExp = Mathf.RoundToInt(maxExp * 1.5f); 

        Debug.Log($"★레벨업! 현재 레벨: {currentLevel}★");

        // 하이러키에 숨겨져 있던 레벨업 패널 활성화!
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