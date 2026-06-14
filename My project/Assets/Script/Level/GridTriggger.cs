using UnityEngine;
using UnityEngine.SceneManagement;

public class GridStageTrigger : MonoBehaviour
{
    [Header("이동할 씬 설정")]
    [Tooltip("이 그리드 영역에 닿았을 때 이동할 씬의 정확한 이름을 입력하세요.")]
    [SerializeField] private string targetSceneName = "";

    [Header("오브젝트 태그 확인")]
    [Tooltip("그리드와 충돌을 감지할 대상의 태그입니다. (기본값: Player)")]
    [SerializeField] private string targetTag = "Player";

    // 2D 콜라이더 영역에 무언가 들어왔을 때 실행되는 함수
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 충돌한 오브젝트의 태그가 플레이어(또는 지정한 태그)인지 확인
        if (collision.CompareTag(targetTag))
        {
            // 씬 이름이 제대로 입력되었는지 검증
            if (!string.IsNullOrEmpty(targetSceneName))
            {
                Debug.Log($"[{gameObject.name}] 그리드 발판 작동! -> {targetSceneName} 씬으로 이동합니다.");

                // 지정된 씬으로 이동
                SceneManager.LoadScene(targetSceneName);
            }
            else
            {
                Debug.LogError($"{gameObject.name} 오브젝트의 이동할 씬 이름(Target Scene Name)이 비어있습니다!");
            }
        }
    }
}