using UnityEngine;
using System.IO;
using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    public int savedScore;
    public int savedHighScore;
    public int savedMaxHp;
    public int savedAttack;
    public List<string> unlockedEvolutions = new List<string>();
}

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance { get; private set; }

    [Header("영구 보존 재화 및 능력치")]
    [SerializeField] private int currentScore = 0;
    [SerializeField] private int highScore = 0;
    [SerializeField] private float playerMoveSpeed = 3f;
    [SerializeField] private int playerMaxHp = 100;
    [SerializeField] private int playerAttack = 15;

    public List<string> unlockedEvolutions = new List<string>();
    private string saveFilePath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            saveFilePath = Application.persistentDataPath + "/playerSaveData.json";
            LoadGameData();
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void StartNewGameRun()
    {
        currentScore = 0;
        playerMaxHp = 100;
        playerAttack = 15;

        SaveGameData();
        Debug.Log("[알림] 새 게임 런 가동: 현재 점수 및 강화 능력치가 완전 초기화되었습니다.");
    }

    public void ResetScore()
    {
        if (InGameUIManager.Instance != null) InGameUIManager.Instance.UpdateScoreUI(currentScore);
    }

    public void AddScore(int amount)
    {
        currentScore += amount;

        if (currentScore > highScore)
        {
            highScore = currentScore;
        }

        if (InGameUIManager.Instance != null) InGameUIManager.Instance.UpdateScoreUI(currentScore);
        SaveGameData();
    }

    public int GetCurrentScore() { return currentScore; }
    public int GetHighScore() { return highScore; }
    public float GetPlayerMoveSpeed() { return playerMoveSpeed; }
    public int GetPlayerHp() { return playerMaxHp; }
    public int GetPlayerAttack() { return playerAttack; }

    public bool SpendScore(int cost)
    {
        if (currentScore >= cost)
        {
            currentScore -= cost;
            if (InGameUIManager.Instance != null) InGameUIManager.Instance.UpdateScoreUI(currentScore);
            SaveGameData();
            return true;
        }
        return false;
    }

    public void AddBaseHp(int amount) { playerMaxHp += amount; SaveGameData(); }
    public void AddBaseAttack(int amount) { playerAttack += amount; SaveGameData(); }

    public void UnlockEvolution(string evoName)
    {
        if (!string.IsNullOrEmpty(evoName) && !unlockedEvolutions.Contains(evoName))
        {
            unlockedEvolutions.Add(evoName);
            SaveGameData();
        }
    }

    public bool IsUnlocked(string evoName) { return unlockedEvolutions.Contains(evoName); }
    public void ClearLibrary()
    {
        unlockedEvolutions.Clear();
        Debug.Log("[도감] 게임 종료 요청으로 인해 도감 해금 데이터가 리셋되었습니다.");
    }
    public void SaveGameData()
    {
        if (currentScore > highScore) highScore = currentScore;

        SaveData data = new SaveData();
        data.savedScore = currentScore;
        data.savedHighScore = highScore;
        data.savedMaxHp = playerMaxHp;
        data.savedAttack = playerAttack;
        data.unlockedEvolutions = new List<string>(unlockedEvolutions);

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(saveFilePath, json);
    }

    public void LoadGameData()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            currentScore = data.savedScore;
            highScore = data.savedHighScore;
            playerMaxHp = data.savedMaxHp;
            playerAttack = data.savedAttack;
            if (data.unlockedEvolutions != null) unlockedEvolutions = data.unlockedEvolutions;
        }
    }
}