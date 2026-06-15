using UnityEngine;

public class ExpGem : MonoBehaviour
{
    [Header("지급 데이터 설정")]
    [Tooltip("이 알갱이가 올려줄 경험치 양")]
    [SerializeField] private int expReward = 10;

    [Tooltip("이 알갱이를 먹었을 때 가산될 점수(Score) 점수")]
    [SerializeField] private int scoreReward = 15;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 중앙 매니저에게 경험치와 점수를 동시에 전달합니다.
            if (PlayerExpManager.Instance != null)
            {
                PlayerExpManager.Instance.AddExperience(expReward, scoreReward);
            }

            Destroy(gameObject);
        }
    }
}