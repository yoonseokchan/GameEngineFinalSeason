using UnityEngine;
using UnityEngine.UI;

public class LevelUpButton : MonoBehaviour
{
    private PlayerEvolutionSO assignedEvolution;

    // 중앙 매니저가 이 버튼에 진화 SO 데이터를 주입할 때 사용
    public void SetupButton(PlayerEvolutionSO evolutionData)
    {
        assignedEvolution = evolutionData;

        Image btnImage = GetComponent<Image>();
        if (btnImage != null && assignedEvolution != null && assignedEvolution.spriteDown.Length > 0)
        {
            btnImage.sprite = assignedEvolution.spriteDown[0];
        }
    }

    // 버튼이 클릭되었을 때 호출될 함수 
    public void OnClickSelection()
    {
        if (assignedEvolution == null) return;

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            PlayerController playerCtrl = player.GetComponent<PlayerController>();
            if (playerCtrl != null)
            {
                playerCtrl.ApplyEvolution(assignedEvolution);
            }
        }
        if (PlayerExpManager.Instance != null)
        {
            PlayerExpManager.Instance.CloseLevelUpPanel();
        }
    }
}