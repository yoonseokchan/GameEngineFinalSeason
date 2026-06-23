using UnityEngine;

public class UpgradeAltar : MonoBehaviour
{
    [Tooltip("씬에 배치된 StatUpgradeUI 스크립트를 연결해주세요.")]
    [SerializeField] private StatUpgradeUI upgradeUI;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (upgradeUI != null)
            {
                upgradeUI.OpenPanel();
            }
        }
    }
}