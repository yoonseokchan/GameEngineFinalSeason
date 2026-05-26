using UnityEngine;

public class LevelUpController : MonoBehaviour
{
    [Header("UI Panel 설정")]
    [SerializeField] private GameObject levelUpPanel; 

    void Start()
    {
        if (levelUpPanel != null)
        {
            levelUpPanel.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 1단계: 무언가와 부딪히긴 했는지 확인
        Debug.Log("무언가와 충돌함: " + collision.gameObject.name);

        if (collision.CompareTag("Player"))
        {
            // 2단계: 그 부딪힌 게 플레이어가 맞는지 확인
            Debug.Log("플레이어 충돌 확인! 패널을 켭니다.");
            OpenPanel();
        }
    }

    public void OpenPanel()
    {
        if (levelUpPanel != null)
        {
            levelUpPanel.SetActive(true);

        }
    }

    public void ClosePanel()
    {
        if (levelUpPanel != null)
        {
            levelUpPanel.SetActive(false);

        }
    }
}