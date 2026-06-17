using UnityEngine;

public class ExpGem : MonoBehaviour
{
    [Header("지급 데이터 설정")]
    [SerializeField] private int expReward = 10;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 중앙 매니저에게 경험치와 점수를 동시에 전달합니다.
            if (PlayerExpManager.Instance != null)
            {
                PlayerExpManager.Instance.AddExperience(expReward);
            }

            Destroy(gameObject);
        }
    }
}