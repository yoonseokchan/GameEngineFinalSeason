using System.IO;
using UnityEditor.Overlays;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance;
    public GameSettingData gameSettingData;
    public SaveData saveData;
    public int isTutorialFinished;

    private string savePath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            savePath = Application.persistentDataPath + "/saveData.json";

            LoadJsonData();
            LoadPlayerPrefs();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public int GetPlayerHp()
    {
        int baseHp = gameSettingData.startHp;
        int bonusHp = gameSettingData.hpBonusPerDeath;

        return baseHp + bonusHp * saveData.deathCount;
    }

    public int GetPlayerAttack()
    {
        int baseAttack = gameSettingData.startAttack;
        int bonusAttack = gameSettingData.atkBonusPerDeath;
        return baseAttack + bonusAttack * saveData.deathCount;
    }

    public float GetPlayerMoveSpeed()
    {
        return gameSettingData.playerMoveSpeed;
    }

    public void SaveGameResult()
    {
        saveData.deathCount++;

        SaveJsonData();
    }

    public void SaveJsonData()
    {
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(savePath, json);

        Debug.Log("JSON 저장 완료: " + savePath);
    }

    public void LoadJsonData()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            saveData = JsonUtility.FromJson<SaveData>(json);
        }
        else
        {
            saveData = new SaveData();
            SaveJsonData();
        }
    }

    public void DeleteJsonData()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
        }

        saveData = new SaveData();
        SaveJsonData();

        Debug.Log("JSON 저장 데이터 삭제");
    }

    public void LoadPlayerPrefs()
    {
        isTutorialFinished = PlayerPrefs.GetInt("TUTORIAL", 0);
    }

    public void SavePlayerPrefs()
    {
        PlayerPrefs.SetInt("TUTORIAL", isTutorialFinished);
        PlayerPrefs.Save();

        Debug.Log("PlayerPrefs 저장 완료");
    }

    public void DeletePlayerPrefs()
    {
        // 튜토리얼 유무 정보 삭제하고 싶을때 해당 함수 사용
        PlayerPrefs.DeleteKey("TUTORIAL");
        LoadPlayerPrefs();

        Debug.Log("PlayerPrefs 삭제 완료");
    }
}