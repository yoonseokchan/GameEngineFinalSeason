using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Player Stats")]
    public int playerMaxHP = 100;
    public int playerCurrentHP = 100;
    public int playerAttack = 0;
    public float moveSpeed = 1f;

    [Header("현재 진화 상태 정보")]
    [SerializeField] private PlayerEvolutionSO currentEvolutionData;
    public PlayerEvolutionSO currentEvolution => currentEvolutionData;

    [SerializeField] private float frameTime = 0.15f;

    [Header("피격 내부 설정")]
    [Tooltip("적과 닿았을 때 몇 초마다 대미지를 입을지 설정합니다.")]
    [SerializeField] private float dmgCooldown = 0.5f;
    private float dmgTimer = 0f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Vector2 input;
    private Vector2 velocity;
    private Sprite[] currentSprites;
    private int frameIndex = 0;
    private float timer = 0f;

    private float attackTimer = 0f;
    private Vector2 lastMoveDirection = Vector2.down;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        if (currentEvolutionData != null)
        {
            ApplyEvolution(currentEvolutionData);
        }

        if (GameDataManager.Instance != null)
        {
            moveSpeed = GameDataManager.Instance.GetPlayerMoveSpeed();
            playerMaxHP = GameDataManager.Instance.GetPlayerHp();
            playerAttack = GameDataManager.Instance.GetPlayerAttack();
        }
        else
        {
            moveSpeed = 1f;
            playerMaxHP = 100;
            playerAttack = 10;
            Debug.LogWarning("GameDataManager를 찾을 수 없어 플레이어 능력치를 기본값으로 세팅합니다.");
        }
    }

    private void Start()
    {
        if (InGameUIManager.Instance != null)
        {
            InGameUIManager.Instance.InitializeHPBar(playerMaxHP);
        }
    }

    public void ApplyEvolution(PlayerEvolutionSO newEvolution)
    {
        if (newEvolution == null) return;

        currentEvolutionData = newEvolution;
        if (GameDataManager.Instance != null)
        {
            GameDataManager.Instance.UnlockEvolution(currentEvolutionData.evolutionName);
        }

        playerAttack = (GameDataManager.Instance != null ? GameDataManager.Instance.GetPlayerAttack() : 10) + currentEvolutionData.attackBonus;

        currentSprites = currentEvolutionData.spriteDown;
        if (currentSprites != null && currentSprites.Length > 0) sr.sprite = currentSprites[0];

        Debug.Log($"[{currentEvolutionData.evolutionName}] 형태로 변신 완료! 무기가 변경되었습니다.");
    }
    public void RefreshStatsFromManager()
    {
        if (GameDataManager.Instance != null)
        {
            int oldMax = playerMaxHP;
            playerMaxHP = GameDataManager.Instance.GetPlayerHp();
            playerCurrentHP += (playerMaxHP - oldMax); 

            playerAttack = GameDataManager.Instance.GetPlayerAttack() + (currentEvolutionData != null ? currentEvolutionData.attackBonus : 0);

            if (InGameUIManager.Instance != null)
            {
                InGameUIManager.Instance.InitializeHPBar(playerMaxHP);
                InGameUIManager.Instance.UpdateHPBar(playerCurrentHP);
            }

            Debug.Log($"[강화 적용] 플레이어 스탯 갱신 완료! (체력: {playerCurrentHP}/{playerMaxHP}, 공격력: {playerAttack})");
        }
    }

    public void OnMove(InputValue value)
    {
        input = value.Get<Vector2>();
        velocity = input.normalized * moveSpeed;

        if (input.sqrMagnitude > 0.01f)
        {
            lastMoveDirection = input.normalized;

            if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
            {
                if (input.x > 0) ChangeSprites(currentEvolutionData.spriteRight);
                else ChangeSprites(currentEvolutionData.spriteLeft);
            }
            else
            {
                if (input.y > 0) ChangeSprites(currentEvolutionData.spriteUp);
                else ChangeSprites(currentEvolutionData.spriteDown);
            }
        }
    }

    private void Update()
    {
        HandleAnimation();
        HandleAutoAttack();

        if (dmgTimer > 0)
            dmgTimer -= Time.deltaTime;
    }

    private void HandleAutoAttack()
    {
        if (currentEvolutionData == null || currentEvolutionData.projectilePrefab == null) return;

        attackTimer += Time.deltaTime;

        if (attackTimer >= currentEvolutionData.attackCooldown)
        {
            FireProjectile();
            attackTimer = 0f;
        }
    }

    private void FireProjectile()
    {
        GameObject projGo = Instantiate(currentEvolutionData.projectilePrefab, transform.position, Quaternion.identity);
        Projectile projectile = projGo.GetComponent<Projectile>();

        if (projectile != null)
        {
            Vector2 targetDirection = GetDirectionToNearestEnemy();
            projectile.Setup(playerAttack, currentEvolutionData.projectileSpeed, targetDirection, currentEvolutionData.knockbackForce);
        }
    }

    private Vector2 GetDirectionToNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemies.Length == 0) return lastMoveDirection;

        GameObject nearestEnemy = null;
        float shortestDistance = Mathf.Infinity;
        Vector2 currentPosition = transform.position;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(currentPosition, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null)
        {
            return ((Vector2)nearestEnemy.transform.position - currentPosition).normalized;
        }

        return lastMoveDirection;
    }

    private void HandleAnimation()
    {
        if (input.sqrMagnitude <= 0.01f)
        {
            frameIndex = 0;
            if (currentSprites != null && currentSprites.Length > 0)
            {
                sr.sprite = currentSprites[frameIndex];
            }
            return;
        }

        timer += Time.deltaTime;
        if (timer >= frameTime)
        {
            timer = 0f;
            frameIndex++;

            if (currentSprites != null && frameIndex >= currentSprites.Length)
            {
                frameIndex = 0;
            }

            if (currentSprites != null && currentSprites.Length > 0)
            {
                sr.sprite = currentSprites[frameIndex];
            }
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }

    private void ChangeSprites(Sprite[] newSprites)
    {
        if (currentSprites == newSprites || newSprites == null || newSprites.Length == 0)
            return;

        currentSprites = newSprites;
        frameIndex = 0;
        timer = 0f;
        sr.sprite = currentSprites[frameIndex];
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && dmgTimer <= 0f)
        {
            int damageTaken = 15;

            playerCurrentHP -= damageTaken;
            dmgTimer = dmgCooldown;

            Debug.Log($"플레이어 피격 감지! 남은 체력: {playerCurrentHP}/{playerMaxHP}");

            if (InGameUIManager.Instance != null)
            {
                InGameUIManager.Instance.UpdateHPBar(playerCurrentHP);
            }

            if (playerCurrentHP <= 0)
            {
                playerCurrentHP = 0;
                if (InGameUIManager.Instance != null)
                {
                    InGameUIManager.Instance.TriggerGameOver();
                }
            }
        }
    }
}