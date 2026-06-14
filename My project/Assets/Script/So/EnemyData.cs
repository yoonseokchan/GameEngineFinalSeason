using UnityEngine;

[CreateAssetMenu(menuName = "Game Data/Game Setting Data")]
public class EnemyStatus : ScriptableObject
{
    public int startHp = 100;
    public int startAttack = 10;
    public float EnemyrMoveSpeed = 0.5f;

    [Header("드롭 설정")]
    [Tooltip("이 적이 죽었을 때 떨어뜨릴 경험치 알갱이 프리팹")]
    public GameObject expGemPrefab;
}