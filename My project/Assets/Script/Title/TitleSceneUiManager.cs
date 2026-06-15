using UnityEngine;
using TMPro;

public class TitleSceneUIManager : MonoBehaviour
{
    [Header("타이틀 UI 텍스트 컴포넌트")]
    [SerializeField] private TextMeshProUGUI lastScoreText; 
    [SerializeField] private TextMeshProUGUI bestScoreText;

    private void Start()
    {

        int lastScore = PlayerPrefs.GetInt("LastMatchScore", 0);
        int bestScore = PlayerPrefs.GetInt("BestScore", 0);

        // 텍스트 UI에 연동해서 출력
        if (lastScoreText != null)
        {
            lastScoreText.text = $"최근 점수: {lastScore}";
        }

        if (bestScoreText != null)
        {
            bestScoreText.text = $"최고 기록: {bestScore}";
        }
    }

    public void ResetAllData()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("모든 점수 기록이 초기화되었습니다.");
    }
}