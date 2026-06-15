using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("적 데이터 설정")]
    [SerializeField] private EnemyStatus enemyData;

    private int currentHp;
    private float moveSpeed;

    private Rigidbody2D rb;
    private Transform playerTransform; 
    private bool isKnockback = false;  

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if (enemyData != null)
        {
            currentHp = enemyData.startHp;
            moveSpeed = enemyData.EnemyrMoveSpeed;
        }
        else
        {
            currentHp = 100;
            moveSpeed = 2f; 
            Debug.LogWarning($"{gameObject.name}: EnemyStatus(SO)가 연결되지 않아 기본값 속도(2f)로 설정합니다.");
        }

        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
            Debug.Log($"{gameObject.name}: 플레이어를 타겟으로 포착했습니다. 위치: {playerTransform.position}");
        }
        else
        {
            Debug.LogError($"{gameObject.name}: 씬에 'Player' 태그를 가진 오브젝트를 찾을 수 없습니다! 이동 불가!");
        }
    }

    private void FixedUpdate()
    {
        // 넉백 중이 아니고, 플레이어 위치를 확보했다면 무조건 이동 함수 실행
        if (!isKnockback && playerTransform != null)
        {
            MoveTowardsPlayer();
        }
        else if (playerTransform == null && !isKnockback)
        {
            // 플레이어를 못 찾았다면 속도를 0으로 만들어 멈춤
            rb.linearVelocity = Vector2.zero;
        }
    }

    private void MoveTowardsPlayer()
    {
        Vector2 direction = ((Vector2)playerTransform.position - rb.position).normalized;

        rb.linearVelocity = direction * moveSpeed;

        if (direction.x > 0.01f) transform.localScale = new Vector3(-1f, 1f, 1f);
        else if (direction.x < -0.01f) transform.localScale = new Vector3(1f, 1f, 1f);
    }

    public void TakeDamage(int damage, Vector2 knockbackDir, float knockbackForce)
    {
        currentHp -= damage;
        if (currentHp <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(KnockbackRoutine(knockbackDir, knockbackForce));
        }
    }

    private IEnumerator KnockbackRoutine(Vector2 dir, float force)
    {
        if (rb == null) yield break;

        isKnockback = true;

        // 넉백 힘 가하기
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(dir * force, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.15f);

        rb.linearVelocity = Vector2.zero;
        isKnockback = false;
    }

    private void Die()
    {
        if (enemyData != null && enemyData.expGemPrefab != null)
        {
            Instantiate(enemyData.expGemPrefab, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}