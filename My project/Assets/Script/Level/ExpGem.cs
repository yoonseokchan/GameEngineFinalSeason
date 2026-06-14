using UnityEngine;

public class ExpGem : MonoBehaviour
{
    [Header("경험치 지급량")]
    [SerializeField] private int expReward = 10; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (PlayerExpManager.Instance != null)
            {
                PlayerExpManager.Instance.AddExperience(expReward);
            }
            else
            {
                Debug.LogWarning("PlayerExpManager를 씬에서 찾을 수 없습니다!");
            }

            // 알갱이 목적 달성 후 파괴
            Destroy(gameObject);
        }
    }
}