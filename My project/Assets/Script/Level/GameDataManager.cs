using UnityEngine;
using System.IO; 

[System.Serializable]
public class SaveData
{
    public int isTutorialFinished;
    public int currentStage;
    public int bestScore;
}

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance { get; private set; }

    public int isTutorialFinished = 0;
    private string saveFilePath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    public void SaveGameDataByJson()
    {
        SaveData data = new SaveData();
        data.isTutorialFinished = this.isTutorialFinished;

        string json = JsonUtility.ToJson(data, true);

        File.WriteAllText(saveFilePath, json);
        Debug.Log($"[JSON 세이브 완료] 저장 경로: {saveFilePath}");
    }

    public void LoadGameDataFromJson()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            this.isTutorialFinished = data.isTutorialFinished;
            Debug.Log("[JSON 로드 완료] 이전 데이터를 성공적으로 불러왔습니다.");
        }
    }
    public void SaveGameResult()
    {
        Debug.Log("GameDataManager: SaveGameResult 함수가 정상적으로 호출되었습니다.");
    }

    [Header("플레이어 기본 로그라이트 능력치")]
    [SerializeField] private float playerMoveSpeed = 3f;
    [SerializeField] private int playerMaxHp = 100;
    [SerializeField] private int playerAttack = 15;

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
}