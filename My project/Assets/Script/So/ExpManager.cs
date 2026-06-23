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

    [Tooltip("하이러키 창에 있는 3개의 카드 버튼 오브젝트를 순서대로 넣어주세요. (왼쪽/가운데/오른쪽 등)")]
    [SerializeField] private LevelUpButton[] optionButtons;

    [Header("최초/기본 진화 데이터 리스트")]
    [Tooltip("최초 레벨 1 플레이어 상태에서 등장할 수 있는 기본 진화 풀입니다. 등록된 순서대로 버튼에 고정 배치됩니다.")]
    [SerializeField] private List<PlayerEvolutionSO> baseEvolutions = new List<PlayerEvolutionSO>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        if (levelUpPanel != null) levelUpPanel.SetActive(false);
    }

    public void AddExperience(int amount)
    {
        currentExp += amount;
        Debug.Log($"경험치 획득: +{amount} (현재: {currentExp}/{maxExp})");

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
            GameDataManager.Instance.SaveGameData();
        }
        RefreshLevelUpOptions();

        if (levelUpPanel != null)
        {
            levelUpPanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }
    private void RefreshLevelUpOptions()
    {
        if (optionButtons == null || optionButtons.Length == 0) return;
        GameObject player = GameObject.FindWithTag("Player");
        PlayerEvolutionSO currentEvolution = null;

        if (player != null)
        {
            PlayerController playerCtrl = player.GetComponent<PlayerController>();
            if (playerCtrl != null)
            {
                currentEvolution = playerCtrl.currentEvolution;
            }
        }
        List<PlayerEvolutionSO> availablePool = new List<PlayerEvolutionSO>();

        if (currentEvolution != null && currentEvolution.nextEvolutions != null && currentEvolution.nextEvolutions.Count > 0)
        {
            availablePool = new List<PlayerEvolutionSO>(currentEvolution.nextEvolutions);
        }
        else
        {
            if (baseEvolutions.Count == 0) return;
            availablePool = new List<PlayerEvolutionSO>(baseEvolutions);
        }
        for (int i = 0; i < optionButtons.Length; i++)
        {
            if (optionButtons[i] == null) continue;
            if (i >= availablePool.Count)
            {
                optionButtons[i].gameObject.SetActive(false);
                continue;
            }
            optionButtons[i].gameObject.SetActive(true);

            PlayerEvolutionSO assignedEvolution = availablePool[i];
            optionButtons[i].SetupButton(assignedEvolution);
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