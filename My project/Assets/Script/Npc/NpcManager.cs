using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EvolutionBookManager : MonoBehaviour
{
    [System.Serializable]
    public struct EvolutionData
    {
        public string evolutionKey; // PlayerPrefs에 저장될 키값 (예: "Unlocked_Evo1")
        public Image uiImage;       // 도감 UI에 배치된 해당 진화형의 Image 컴포넌트
    }

    [Header("대화 UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private string npcDialogue = "자네가 도감을 보러 왔구먼. 지금까지 발견한 진화 모습들이라네!";

    [Header("NPC 일러스트 설정")]
    [Tooltip("대화창 우측에 배치한 NPC 일러스트용 UI Image 컴포넌트를 연결하세요.")]
    [SerializeField] private Image npcPortraitImage;

    [Tooltip("이 NPC가 대화할 때 보여줄 일러스트 스프라이트를 넣어주세요.")]
    [SerializeField] private Sprite npcSprite;

    [Header("도감 UI")]
    [SerializeField] private GameObject bookPanel;
    [SerializeField] private EvolutionData[] evolutionList;

    private void Start()
    {
        // 게임 시작 시 UI들은 꺼둡니다.
        dialoguePanel.SetActive(false);
        bookPanel.SetActive(false);

        if (npcPortraitImage != null)
        {
            npcPortraitImage.gameObject.SetActive(false);
        }
    }

    // NPC 트리거에 닿았을 때 호출될 함수
    public void StartNPCOnClick()
    {
        dialoguePanel.SetActive(true);
        dialogueText.text = npcDialogue;

        // NPC 일러스트 이미지 컴포넌트와 스프라이트가 둘 다 등록되어 있다면 띄우기
        if (npcPortraitImage != null && npcSprite != null)
        {
            npcPortraitImage.sprite = npcSprite;
            npcPortraitImage.gameObject.SetActive(true);
            Debug.Log("NPC 일러스트를 화면에 표시합니다.");
        }
    }

    // 대화창을 누르거나 대화가 끝났을 때 도감창을 여는 함수
    public void OpenEvolutionBook()
    {
        dialoguePanel.SetActive(false);

        // 도감 창을 열 때는 대화용 NPC 일러스트를 꺼줍니다.
        if (npcPortraitImage != null)
        {
            npcPortraitImage.gameObject.SetActive(false);
        }

        bookPanel.SetActive(true);
        UpdateBookUI();
    }

    // 도감 UI 새로고침 (실루엣 vs 원본)
    private void UpdateBookUI()
    {
        foreach (var evo in evolutionList)
        {
            bool isUnlocked = PlayerPrefs.GetInt(evo.evolutionKey, 0) == 1;

            if (isUnlocked)
            {
                evo.uiImage.color = Color.white;
            }
            else
            {
                evo.uiImage.color = Color.black;
            }
        }
    }

    // 도감 닫기 버튼용
    public void CloseBook()
    {
        bookPanel.SetActive(false);
        Time.timeScale = 1f; // 일시정지 해제
    }
}