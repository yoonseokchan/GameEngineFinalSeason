using UnityEngine;
using TMPro;

public class StatUpgradeUI : MonoBehaviour
{
    [Header("강화 패널 설정")]
    [SerializeField] private GameObject upgradePanel;

    [Header(" 실시간 스탯 텍스트 UI 설정")]
    [Tooltip("하트 아이콘 위에 있는 'New Text' 오브젝트를 연결해주세요.")]
    [SerializeField] private TextMeshProUGUI hpStatText;

    [Tooltip("칼 아이콘 위에 있는 'New Text' 오브젝트를 연결해주세요.")]
    [SerializeField] private TextMeshProUGUI atkStatText;

    [Header("강화 수치 및 비용 설정")]
    [SerializeField] private int upgradeCost = 200;
    [SerializeField] private int hpIncreaseAmount = 20; 
    [SerializeField] private int atkIncreaseAmount = 5;  

    private void Start()
    {
        if (upgradePanel != null) upgradePanel.SetActive(false);
    }

    public void OpenPanel()
    {
        if (upgradePanel != null)
        {
            upgradePanel.SetActive(true);
            Time.timeScale = 0f; 
            UpdateStatTexts();
        }
    }
    public void ClosePanel()
    {
        if (upgradePanel != null)
        {
            upgradePanel.SetActive(false);
            Time.timeScale = 1f; // 게임 다시 재생
        }
    }
    public void OnClickHpUpgrade()
    {
        if (GameDataManager.Instance != null && GameDataManager.Instance.SpendScore(upgradeCost))
        {
            GameDataManager.Instance.AddBaseHp(hpIncreaseAmount);
            ApplyToPlayer();
            UpdateStatTexts();
        }
    }
    public void OnClickAtkUpgrade()
    {
        if (GameDataManager.Instance != null && GameDataManager.Instance.SpendScore(upgradeCost))
        {
            GameDataManager.Instance.AddBaseAttack(atkIncreaseAmount);
            ApplyToPlayer();
            UpdateStatTexts();
        }
    }

    private void ApplyToPlayer()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            PlayerController playerCtrl = player.GetComponent<PlayerController>();
            if (playerCtrl != null) playerCtrl.RefreshStatsFromManager();
        }
    }

    private void UpdateStatTexts()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            PlayerController playerCtrl = player.GetComponent<PlayerController>();
            if (playerCtrl != null)
            {
                if (hpStatText != null)
                {
                    hpStatText.text = $"체력: {playerCtrl.playerMaxHP}";
                }
                if (atkStatText != null)
                {
                    atkStatText.text = $"공격력: {playerCtrl.playerAttack}";
                }
            }
        }
        else
        {
            Debug.LogWarning("[강화 UI] 플레이어를 찾을 수 없어 스탯을 표시할 수 없습니다.");
        }
    }
}