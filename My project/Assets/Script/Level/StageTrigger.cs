using UnityEngine;
using UnityEngine.SceneManagement;

public class StageTrigger : MonoBehaviour
{
    public enum TriggerMode { StageChange, CustomSystem }

    [Header("트리거 모드 설정")]
    [SerializeField] private TriggerMode triggerMode = TriggerMode.StageChange;

    [Header("공통 설정")]
    [Tooltip("이동할 씬 이름을 적어주세요. (예: Level_2 또는 Scene_Encyclopedia)")]
    [SerializeField] private string targetSceneName = "";

    [Header("스테이지 변경 모드 전용")]
    [Tooltip("스테이지 이동 시 변경될 데이터 번호입니다.")]
    [SerializeField] private int nextStageNumber = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (string.IsNullOrEmpty(targetSceneName))
            {
                Debug.LogError($"{gameObject.name}: 이동할 씬 이름이 비어있습니다!");
                return;
            }

            if (triggerMode == TriggerMode.StageChange)
            {
                // [기존 방식] 데이터를 저장하고 아예 씬을 새로 갈아끼움
                PlayerPrefs.SetInt("CurrentStage", nextStageNumber);
                PlayerPrefs.Save();
                SceneManager.LoadScene(targetSceneName);
            }
            else if (triggerMode == TriggerMode.CustomSystem)
            {
                // [방안 2 적용] 메인 씬은 그대로 두고 그 위에 도감/외형변경 씬을 '얹음'
                Debug.Log($"{targetSceneName} 시스템을 중첩 로드합니다.");

                // 1. 현재 메인 게임을 잠시 멈춤 (시간 정지 혹은 플레이어 조작 제한)
                // 가장 간단하게 게임을 일시정지 시키는 방법입니다.
                Time.timeScale = 0f;

                // 2. Additive 모드로 씬 로드
                SceneManager.LoadScene(targetSceneName, LoadSceneMode.Additive);
            }
        }
    }
}   