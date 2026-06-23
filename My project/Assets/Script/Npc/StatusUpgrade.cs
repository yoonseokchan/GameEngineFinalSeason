using UnityEngine;

public class StatUpgradeUI : MonoBehaviour
{
    [Header("강화 패널 설정")]
    [SerializeField] private GameObject upgradePanel;

    [Header("강화 수치 및 비용 설정")]
    [SerializeField] private int upgradeCost = 200;
    [SerializeField] private int hpIncreaseAmount = 20; 
    [SerializeField] private int atkIncreaseAmount = 5; 

    private void Start()
    {
        if (upgradePanel != null) upgradePanel.SetActive(false);
    }

    //  패널 열기
    public void OpenPanel()
    {
        if (upgradePanel != null)
        {
            upgradePanel.SetActive(true);
            Time.timeScale = 0f; // 게임 일시정지
        }
    }

    // 패널 닫기 
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
        }
    }
    public void OnClickAtkUpgrade()
    {
        if (GameDataManager.Instance != null && GameDataManager.Instance.SpendScore(upgradeCost))
        {
            GameDataManager.Instance.AddBaseAttack(atkIncreaseAmount);
            ApplyToPlayer();
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
}