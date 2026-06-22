using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance { get; private set; }

    [Header("실시간 점수 데이터")]
    [SerializeField] private int currentScore = 0;

    [Header("플레이어 기본 로그라이트 능력치")]
    [SerializeField] private float playerMoveSpeed = 3f;
    [SerializeField] private int playerMaxHp = 100;
    [SerializeField] private int playerAttack = 15;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
            Debug.Log("GameDataManager: 최초 인스턴스가 성공적으로 생성되었으며 파괴 방지 설정되었습니다.");
        }
        else if (Instance != this)
        {
            Debug.Log($"GameDataManager: 중복된 매니저 오브젝트({gameObject.name})를 파괴하여 싱글톤을 유지합니다.");
            Destroy(gameObject);
        }
    }

    public void ResetScore()
    {
        currentScore = 0;
        if (InGameUIManager.Instance != null)
        {
            InGameUIManager.Instance.UpdateScoreUI(currentScore);
        }
    }

    public void AddScore(int amount)
    {
        currentScore += amount;
        Debug.Log($"[DataManager] 점수 획득! +{amount} (현재 총 점수: {currentScore})");

        if (InGameUIManager.Instance != null)
        {
            InGameUIManager.Instance.UpdateScoreUI(currentScore);
        }
    }

    public int GetCurrentScore()
    {
        return currentScore;
    }

    public float GetPlayerMoveSpeed()
    {
        return playerMoveSpeed;
    }

    public int GetPlayerHp()
    {
        return playerMaxHp;
    }

    public int GetPlayerAttack()
    {
        return playerAttack;
    }

    public void SaveGameResult()
    {
        Debug.Log($"[JSON 예비] 현재 점수 {currentScore}점으로 데이터 동기화 및 JSON 세이브 파일 기록 트리거.");
    }
}