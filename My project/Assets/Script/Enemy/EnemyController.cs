using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("적 데이터 설정")]
    [SerializeField] private EnemyStatus enemyData;

    private int currentHp;
    private float moveSpeed;

    private Rigidbody2D rb;
    private Transform playerTransform; // 추적할 플레이어의 위치 정보
    private bool isKnockback = false;  // 넉백 중인지 체크하는 플래그

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // 1. ScriptableObject 데이터 기반으로 능력치 초기화
        if (enemyData != null)
        {
            currentHp = enemyData.startHp;
            moveSpeed = enemyData.EnemyrMoveSpeed;
        }
        else
        {
            currentHp = 100;
            moveSpeed = 1f;
            Debug.LogWarning($"{gameObject.name}: EnemyStatus(SO)가 연결되지 않아 기본값으로 설정합니다.");
        }

        // 2. 씬에 있는 플레이어 오브젝트를 "Player" 태그로 실시간 탐색
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
        }
        else
        {
            Debug.LogError($"{gameObject.name}: 씬에 'Player' 태그를 가진 오브젝트가 없습니다!");
        }
    }

    private void FixedUpdate()
    {
        // 넉백 중이 아니고, 플레이어가 씬에 존재할 때만 추적 이동 진행
        if (!isKnockback && playerTransform != null)
        {
            MoveTowardsPlayer();
        }
    }

    private void MoveTowardsPlayer()
    {
        // 플레이어가 있는 방향 계산 (목적지 - 내 위치)
        Vector2 direction = (playerTransform.position - transform.position).normalized;

        // 물리(Rigidbody2D)를 사용하여 플레이어 방향으로 안정적으로 이동
        Vector2 nextPosition = rb.position + direction * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(nextPosition);

        // [선택 사항] 적의 진행 방향에 따라 좌우 반전(Flip) 연출
        if (direction.x > 0.01f) transform.localScale = new Vector3(-1f, 1f, 1f); // 우측 타겟
        else if (direction.x < -0.01f) transform.localScale = new Vector3(1f, 1f, 1f); // 좌측 타겟
    }

    // 투사체(Projectile)에 맞았을 때 호출되는 대미지 및 넉백 함수
    public void TakeDamage(int damage, Vector2 knockbackDir, float knockbackForce)
    {
        currentHp -= damage;
        Debug.Log($"적 대미지 발생! 남은 체력: {currentHp}");

        if (currentHp <= 0)
        {
            Die();
        }
        else
        {
            // 아직 살아있다면 추적을 멈추고 밀려나는 코루틴 실행
            StartCoroutine(KnockbackRoutine(knockbackDir, knockbackForce));
        }
    }

    private IEnumerator KnockbackRoutine(Vector2 dir, float force)
    {
        if (rb == null) yield break;

        isKnockback = true;

        // 순간적으로 밀려나는 물리 힘 가하기
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(dir * force, ForceMode2D.Impulse);

        // 0.15초 동안 넉백 상태 유지 (이 동안은 플레이어 추적 안 함)
        yield return new WaitForSeconds(0.15f);

        // 속도 초기화 후 다시 플레이어 추적 재개
        rb.linearVelocity = Vector2.zero;
        isKnockback = false;
    }

    private void Die()
    {
        // 사망 시 경험치 알갱이 스폰
        if (enemyData != null && enemyData.expGemPrefab != null)
        {
            Instantiate(enemyData.expGemPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}