using UnityEngine;

public class LibraryAltar : MonoBehaviour
{
    [Tooltip("씬에 배치된 LibraryUI 스크립트를 연결해주세요.")]
    [SerializeField] private LibraryUI libraryUI;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (libraryUI != null)
            {
                libraryUI.OpenLibrary();
            }
        }
    }
}