using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseAndGameOverManager : MonoBehaviour
{
    public static PauseAndGameOverManager Instance { get; private set; }

    [Header("UI 패널 연결 (하이러키에서 드래그)")]
    [SerializeField] private GameObject pausePanel;       // 일시중단 패널 
    [SerializeField] private GameObject settingsPanel;    // 옵션 설정 패널 
    [SerializeField] private GameObject gameOverPanel;    // 사망 패널 

    private bool isPaused = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // 게임 시작 시 모든 제어 패널은 비활성화
        if (pausePanel != null) pausePanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (settingsPanel != null && settingsPanel.activeSelf)
            {
                CloseOptions();
            }
            else if (gameOverPanel == null || !gameOverPanel.activeSelf)
            {
                if (isPaused) ResumeGame();
                else PauseGame();
            }
        }
    }
    public void RestartGame()
    {
        Time.timeScale = 1f; // 일시정지 전역 해제 

        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);

        Debug.Log($"{currentSceneName} 씬을 재시작합니다. 인게임 스코어 데이터는 초기화됩니다.");
    }

    public void PauseGame()
    {
        isPaused = true;
        if (pausePanel != null) pausePanel.SetActive(true);
        Time.timeScale = 0f; 
    }

    public void ResumeGame()
    {
        isPaused = false;
        if (pausePanel != null) pausePanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);
        Time.timeScale = 1f;
    }
    public void ExitToTitle()
    {
        Time.timeScale = 1f; // 일시정지 전역 해제

        PlayerPrefs.SetInt("LastMatchScore", 0);
        PlayerPrefs.Save();

        Debug.Log("인게임 데이터를 저장하지 않고 타이틀 화면으로.");
        SceneManager.LoadScene("MapSelect"); 
    }

    public void OpenOptions()
    {
        if (pausePanel != null) pausePanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(true);
    }

    public void CloseOptions()
    {
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(true); // 다시 일시중단 메뉴 패널 
    }
    public void Title()
    {
        SceneManager.LoadScene("Title");
    }
}