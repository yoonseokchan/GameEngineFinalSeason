using UnityEngine;
using UnityEngine.SceneManagement;

public class StageTrigger : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string stageScenePrefix = "Level_"; // 씬 이름의 앞부분 (예: Level_1, Level_2)

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("플레이어가 다음 스테이지 영역에 도달했습니다!");

            int currentStage = PlayerPrefs.GetInt("CurrentStage", 1);

            int nextStage = currentStage + 1;

            PlayerPrefs.SetInt("CurrentStage", nextStage);
            PlayerPrefs.Save();

            string nextSceneName = stageScenePrefix + nextStage;

            Debug.Log("다음 이동할 씬: " + nextSceneName);

            SceneManager.LoadScene(nextSceneName);
        }
    }
}
