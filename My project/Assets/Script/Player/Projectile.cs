using UnityEngine;

public class Projectile : MonoBehaviour
{
    private int damage;
    private float knockbackForce;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // 플레이어가 투사체를 만들 때 호출하여 값을 주입하는 함수
    public void Setup(int dmg, float speed, Vector2 dir, float kForce)
    {
        damage = dmg;
        knockbackForce = kForce;

        // 투사체를 바라보는 방향으로 이동시킴
        if (rb != null)
        {
            rb.linearVelocity = dir * speed;
        }

        // Z축 회전을 주어 날아가는 방향을 바라보게 연출 (선택사항)
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // 5초 지나면 화면 밖으로 나갔다고 판단하고 자동 삭제
        Destroy(gameObject, 5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            // EnemyController 스크립트가 적에게 있다고 가정
            EnemyController enemy = collision.GetComponent<EnemyController>();
            if (enemy != null)
            {
                // 계산된 날아가는 방향(Velocity의 정규화값)을 넉백 방향으로 전달
                Vector2 knockbackDirection = rb.linearVelocity.normalized;

                // 적에게 대미지와 넉백 전달
                enemy.TakeDamage(damage, knockbackDirection, knockbackForce);
            }

            // 적과 부딪혔으므로 투사체 소멸
            Destroy(gameObject);
        }
    }
}