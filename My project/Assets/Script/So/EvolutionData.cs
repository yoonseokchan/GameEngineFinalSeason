using UnityEngine;
using System.Collections.Generic; // List 사용용

[CreateAssetMenu(fileName = "NewEvolution", menuName = "ScriptableObjects/PlayerEvolution")]
public class PlayerEvolutionSO : ScriptableObject
{
    [Header("진화 형태 기본 정보")]
    [Tooltip("인게임 로그창이나 UI에 표시될 진화 형태의 이름입니다. (예: 슬라임, 뱀 등)")]
    public string evolutionName;

    [Header("외형 및 애니메이션 데이터")]
    public Sprite[] spriteUp;
    public Sprite[] spriteDown;
    public Sprite[] spriteLeft;
    public Sprite[] spriteRight;

    [Header("전투 및 능력치 가산 데이터")]
    [Tooltip("이 진화 형태로 변했을 때 보너스로 추가될 공격력 수치입니다.")]
    public int attackBonus = 0;

    [Tooltip("공격 주기(쿨타임) 설정입니다.")]
    public float attackCooldown = 1f;

    [Tooltip("이 진화 형태가 자동으로 발사할 투사체(발사체) 프리패브입니다.")]
    public GameObject projectilePrefab;

    [Tooltip("투사체의 날아가는 속도입니다.")]
    public float projectileSpeed = 5f;

    [Tooltip("투사체가 적에게 적중했을 때 밀어내는 힘의 크기입니다.")]
    public float knockbackForce = 2f;

    [Header("상위 진화 트리 설정")]
    [Tooltip("현재 형태에서 레벨업했을 때 등장할 수 있는 다음 단계 진화 후보 SO들입니다.")]
    public List<PlayerEvolutionSO> nextEvolutions = new List<PlayerEvolutionSO>();
}