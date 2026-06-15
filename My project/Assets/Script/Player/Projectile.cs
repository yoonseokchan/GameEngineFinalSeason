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

    // 플레이어가 계산해서 넘겨준 '가장 가까운 적의 방향(dir)'을 받아서 발사합니다.
    public void Setup(int dmg, float speed, Vector2 dir, float kForce)
    {
        damage = dmg;
        knockbackForce = kForce;

        if (rb != null)
        {
            // 전달받은 적의 방향 * 속도로 투사체를 날립니다.
            rb.linearVelocity = dir * speed;
        }

        // 투사체 이미지가 날아가는 방향(적의 방향)을 정면으로 바라보도록 Z축 회전 연출
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // 화면 밖으로 나갔을 때를 대비해 5초 뒤 자동 소멸
        Destroy(gameObject, 5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 적과 부딪혔을 때 대미지와 넉백 주기
        if (collision.CompareTag("Enemy"))
        {
            EnemyController enemy = collision.GetComponent<EnemyController>();
            if (enemy != null)
            {
                // 투사체가 날아가던 힘의 방향으로 적을 밀어냅니다.
                Vector2 knockbackDirection = rb.linearVelocity.normalized;
                enemy.TakeDamage(damage, knockbackDirection, knockbackForce);
            }

            // 적을 관통하지 않고 부딪히면 소멸하는 투사체 기준입니다.
            Destroy(gameObject);
        }
    }
}