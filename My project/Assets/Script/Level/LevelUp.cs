using UnityEngine;

public class LevelUpController : MonoBehaviour
{
    [Header("UI Panel 설정")]
    [Tooltip("화면에 띄울 레벨업 패널의 부모 오브젝트(Canvas 내)를 연결하세요.")]
    [SerializeField] private GameObject levelUpPanel;

    void Start()
    {
        if (levelUpPanel != null)
        {
            levelUpPanel.SetActive(false);
        }
        else
        {
            levelUpPanel = GameObject.FindWithTag("LevelUpPanel");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 1단계: 무언가와 부딪히긴 했는지 확인
        Debug.Log("무언가와 충돌함: " + collision.gameObject.name);

        if (collision.CompareTag("Player"))
        {
            // 2단계: 그 부딪힌 게 플레이어가 맞는지 확인
            Debug.Log("플레이어 충돌 확인! 레벨업 패널을 켭니다.");
            OpenPanel();

            // [추가] 플레이어가 알갱이를 '획득'했으므로 알갱이 오브젝트는 파괴합니다.
            Destroy(gameObject);
        }
    }

    public void OpenPanel()
    {
        if (levelUpPanel != null)
        {
            levelUpPanel.SetActive(true);

            // 레벨업 보상을 고르는 동안 게임을 잠시 멈추고 싶다면 일시정지를 넣습니다.
            Time.timeScale = 0f;
        }
    }

    // 이 함수는 나중에 레벨업 UI 안의 '능력치 선택 버튼'에 연결해서 사용하세요!
    public void ClosePanel()
    {
        if (levelUpPanel != null)
        {
            levelUpPanel.SetActive(false);

            // 보상을 선택했으므로 게임을 다시 재개합니다.
            Time.timeScale = 1f;
        }
    }
}