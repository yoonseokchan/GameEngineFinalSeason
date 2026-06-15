using UnityEngine;
using UnityEngine.SceneManagement;

public class StageTrigger : MonoBehaviour
{
    [Header("포탈 이동 설정")]
    [Tooltip("이 영역에 닿았을 때 바로 이동할 타겟 씬 이름을 적어주세요. (예: Title, Library)")]
    [SerializeField] private string targetSceneName = "";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!string.IsNullOrEmpty(targetSceneName))
            {
                Debug.Log($"[{gameObject.name}] 작동 -> {targetSceneName} 씬으로 간다.");

                // 지정된 특정 씬으로 즉시 전환
                SceneManager.LoadScene(targetSceneName);
            }
            else
            {
                Debug.LogError($"{gameObject.name}: 씬 비었다 언능 채우자.");
            }
        }
    }
}