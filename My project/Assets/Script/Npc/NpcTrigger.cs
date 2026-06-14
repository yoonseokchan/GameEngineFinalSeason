using UnityEngine;

public class NPCTrigger : MonoBehaviour
{
    [Header("연결할 매니저")]
    [SerializeField] private EvolutionBookManager bookManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("NPC 그리드에 플레이어가 도달했습니다.");

            // 움직임 제어를 위해 게임 일시정지 (선택 사항)
            Time.timeScale = 0f;

            // 대화 시작
            if (bookManager != null)
            {
                bookManager.StartNPCOnClick();
            }
        }
    }
}   