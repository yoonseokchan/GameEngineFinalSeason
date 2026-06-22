using UnityEngine;

public class ExpGem : MonoBehaviour
{
    [SerializeField] private int expReward = 10;
    [SerializeField] private int scoreReward = 15; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (PlayerExpManager.Instance != null)
            {
                PlayerExpManager.Instance.AddExperience(expReward);
            }
            if (GameDataManager.Instance != null)
            {
                GameDataManager.Instance.AddScore(scoreReward);
            }

            Destroy(gameObject);
        }
    }
}