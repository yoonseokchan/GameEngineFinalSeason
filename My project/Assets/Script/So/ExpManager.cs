using UnityEngine;
using System.Collections.Generic; 

public class PlayerExpManager : MonoBehaviour
{
    public static PlayerExpManager Instance { get; private set; }

    [Header("실시간 플레이어 레벨 및 경험치 데이터")]
    public int currentLevel = 1;
    public int currentExp = 0;
    public int maxExp = 100;

    [Header("레벨업 UI 패널 설정")]
    [SerializeField] private GameObject levelUpPanel;

    [Tooltip("하이러키 창에 있는 3개의 카드 버튼 오브젝트를 순서대로 넣어주세요.")]
    [SerializeField] private LevelUpButton[] optionButtons;

    [Header("전체 진화 데이터 리스트")]
    [Tooltip("게임에 존재하는 모든 PlayerEvolutionSO 에셋들을 여기에 전부 등록하세요.")]
    [SerializeField] private List<PlayerEvolutionSO> allEvolutions = new List<PlayerEvolutionSO>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (levelUpPanel != null) levelUpPanel.SetActive(false);
    }
    public void AddExperience(int amount)
    {
        currentExp += amount;
        Debug.Log($"경험치 획득: +{amount} (현재: {currentExp}/{maxExp})");

        // 경험치가 가득 차면 레벨업 루프 가동
        while (currentExp >= maxExp)
        {
            LevelUp();
        }
    }
    private void LevelUp()
    {
        currentLevel++;
        currentExp -= maxExp;
        maxExp = Mathf.RoundToInt(maxExp * 1.3f);

        Debug.Log($"★ 레벨업 달성! 현재 레벨: {currentLevel} ★");
        if (GameDataManager.Instance != null)
        {
            GameDataManager.Instance.SaveGameResult();
        }

        RefreshLevelUpOptions();
        // 시간을 멈추고 화면에 선택 창을 띄움
        if (levelUpPanel != null)
        {
            levelUpPanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }
    private void RefreshLevelUpOptions()
    {
        if (allEvolutions.Count == 0 || optionButtons == null || optionButtons.Length < 3) return;

        List<PlayerEvolutionSO> tempList = new List<PlayerEvolutionSO>(allEvolutions);
        for (int i = 0; i < optionButtons.Length; i++)
        {
            if (tempList.Count == 0) break;

            int randomIndex = Random.Range(0, tempList.Count);
            PlayerEvolutionSO chosenEvolution = tempList[randomIndex];

            if (optionButtons[i] != null)
            {
                optionButtons[i].SetupButton(chosenEvolution);
            }
            tempList.RemoveAt(randomIndex);
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