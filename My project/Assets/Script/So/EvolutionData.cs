using UnityEngine;

[CreateAssetMenu(menuName = "Game Data/Player Evolution Data")]
public class PlayerEvolutionSO : ScriptableObject
{
    [Header("진화 형태 이름")]
    public string evolutionName;

    [Header("능력치 보너스")]
    public int hpBonus = 0;
    public int attackBonus = 10;
    public float speedBonus = 0f;

    [Header("방향별 애니메이션 스프라이트 세트")]
    public Sprite[] spriteUp;
    public Sprite[] spriteDown;
    public Sprite[] spriteLeft;
    public Sprite[] spriteRight;

    [Header("무기 설정")]
    public GameObject projectilePrefab; // 이 형태가 발사할 투사체 프리팹
    public float attackCooldown = 0.5f;  // 공격 속도 (초 단위)
    public float projectileSpeed = 5f;   // 투사체 속도
    public float knockbackForce = 5f;    // 적을 밀어내는 힘
}