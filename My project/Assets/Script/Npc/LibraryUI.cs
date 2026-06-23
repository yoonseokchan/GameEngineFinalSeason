using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct LibrarySlot
{
    public PlayerEvolutionSO targetEvolution;
    public Image iconImage;
}

public class LibraryUI : MonoBehaviour
{
    [Header("도감 패널 설정")]
    [SerializeField] private GameObject libraryPanel;

    [Header("도감 슬롯 목록")]
    public LibrarySlot[] librarySlots;
    
    private void Awake()
    {
        if (libraryPanel != null)
        {
            libraryPanel.SetActive(false);
        }
    }

    public void OpenLibrary()
    {
        if (libraryPanel != null)
        {
            libraryPanel.SetActive(true);
            Time.timeScale = 0f;
            RefreshLibraryUI();
        }
    }

    public void CloseLibrary()
    {
        if (libraryPanel != null)
        {
            libraryPanel.SetActive(false);
            Time.timeScale = 1f; 
        }
    }

    private void RefreshLibraryUI()
    {
        if (GameDataManager.Instance == null) return;

        foreach (LibrarySlot slot in librarySlots)
        {
            if (slot.targetEvolution == null || slot.iconImage == null) continue;

            if (slot.targetEvolution.spriteDown.Length > 0)
            {
                slot.iconImage.sprite = slot.targetEvolution.spriteDown[0];
            }

            bool isUnlocked = GameDataManager.Instance.IsUnlocked(slot.targetEvolution.evolutionName);

            if (isUnlocked)
            {
                slot.iconImage.color = Color.white;
            }
            else
            {
                slot.iconImage.color = Color.black;
            }
        }
    }
}