using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Player Stats")]
    public int playerHP = 0;
    public int playerAttack = 0;
    public float moveSpeed = 1f;

    [Header("현재 진화 상태 정보")]
    [SerializeField] private PlayerEvolutionSO currentEvolution; // 현재 내 진화 에셋
    [SerializeField] private float frameTime = 0.15f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Vector2 input;
    private Vector2 velocity;
    private Sprite[] currentSprites;
    private int frameIndex = 0;
    private float timer = 0f;

    // 공격 관련 타이머 및 방향 기억
    private float attackTimer = 0f;
    private Vector2 lastMoveDirection = Vector2.down; // 기본 공격 방향은 아래

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        // 최초 진화 정보 로드 및 능력치 세팅
        if (currentEvolution != null)
        {
            ApplyEvolution(currentEvolution);
        }

        if (GameDataManager.Instance != null)
        {
            moveSpeed += GameDataManager.Instance.GetPlayerMoveSpeed();
            playerHP += GameDataManager.Instance.GetPlayerHp();
            playerAttack += GameDataManager.Instance.GetPlayerAttack();
        }
    }

    private void Start()
    {
        if (GameDataManager.Instance != null && GameDataManager.Instance.isTutorialFinished == 0)
        {
            GameDataManager.Instance.isTutorialFinished = 1;
        }
    }

    // ?? 레벨업 창에서 새로운 진화 형태를 골랐을 때 실시간으로 호출해 줄 함수
    public void ApplyEvolution(PlayerEvolutionSO newEvolution)
    {
        if (newEvolution == null) return;

        currentEvolution = newEvolution;

        // 능력치 반영 (기본 능력치 + 진화 보너스)
        playerAttack = (GameDataManager.Instance != null ? GameDataManager.Instance.GetPlayerAttack() : 10) + currentEvolution.attackBonus;

        // 무조건 현재 바라보던 방향 기준으로 애니메이션 교체되도록 강제 리셋
        currentSprites = currentEvolution.spriteDown;
        if (currentSprites != null && currentSprites.Length > 0) sr.sprite = currentSprites[0];

        Debug.Log($"[{currentEvolution.evolutionName}] 형태로 변신 완료! 무기가 변경되었습니다.");
    }

    public void OnMove(InputValue value)
    {
        input = value.Get<Vector2>();
        velocity = input.normalized * moveSpeed;

        if (input.sqrMagnitude > 0.01f)
        {
            lastMoveDirection = input.normalized; // 마지막으로 움직인 방향 기억 (공격 방향용)

            if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
            {
                if (input.x > 0) ChangeSprites(currentEvolution.spriteRight);
                else ChangeSprites(currentEvolution.spriteLeft);
            }
            else
            {
                if (input.y > 0) ChangeSprites(currentEvolution.spriteUp);
                else ChangeSprites(currentEvolution.spriteDown);
            }
        }
    }

    private void Update()
    {
        HandleAnimation();
        HandleAutoAttack(); // 매 프레임 공격 쿨타임 계산
    }

    private void HandleAutoAttack()
    {
        if (currentEvolution == null || currentEvolution.projectilePrefab == null) return;

        attackTimer += Time.deltaTime;

        // 현재 진화 형태에 설정된 쿨타임 주기에 도달하면 발사
        if (attackTimer >= currentEvolution.attackCooldown)
        {
            FireProjectile();
            attackTimer = 0f;
        }
    }

    private void FireProjectile()
    {
        // 내 위치에 투사체 생성
        GameObject projGo = Instantiate(currentEvolution.projectilePrefab, transform.position, Quaternion.identity);
        Projectile projectile = projGo.GetComponent<Projectile>();

        if (projectile != null)
        {
            // 투사체 스크립트에 대미지, 속도, 방향, 넉백 힘을 넘겨주어 발사시킵니다.
            projectile.Setup(playerAttack, currentEvolution.projectileSpeed, lastMoveDirection, currentEvolution.knockbackForce);
        }
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (GameManager.Instance != null) GameManager.Instance.GameOver();
        }
    }
}