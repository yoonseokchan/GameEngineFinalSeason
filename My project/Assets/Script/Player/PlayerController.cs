using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Player Stats")]
    public int playerHP = 0;
    public int playerAttack = 0;
    public float moveSpeed = 1f;

    [Header("Animation Sprites")]
    public Sprite[] spriteUp;
    public Sprite[] spriteDown;
    public Sprite[] spriteLeft;
    public Sprite[] spriteRight;
    public float frameTime = 0.15f;

    // 내부 제어용 컴포넌트 및 변수
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Vector2 input;
    private Vector2 velocity;
    private Sprite[] currentSprites;
    private int frameIndex = 0;
    private float timer = 0f;

    private void Awake()
    {
        // 1. 필수 컴포넌트 가져오기
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        // 2. 초기 스프라이트 방향 설정 (아래 방향)
        currentSprites = spriteDown;
        if (currentSprites != null && currentSprites.Length > 0)
        {
            sr.sprite = currentSprites[0];
        }

        // 3. GameDataManager 싱글톤을 통한 로그라이트 능력치 연동
        if (GameDataManager.Instance != null)
        {
            moveSpeed = GameDataManager.Instance.GetPlayerMoveSpeed();
            playerHP = GameDataManager.Instance.GetPlayerHp();
            playerAttack = GameDataManager.Instance.GetPlayerAttack();
        }
        else
        {
            Debug.LogWarning("GameDataManager 인스턴스를 찾을 수 없어 기본값으로 초기화합니다.");
        }
    }

    private void Start()
    {
        // 4. 튜토리얼 진행 유무 판정 로직 결합
        if (GameDataManager.Instance != null)
        {
            if (GameDataManager.Instance.isTutorialFinished == 0)
            {
                // 튜토리얼 안 했을 경우 튜토리얼 오픈
                Debug.Log("튜토리얼 오픈!");
                GameDataManager.Instance.isTutorialFinished = 1;
                // 필요하다면 여기에 튜토리얼 UI를 켜는 코드 추가
            }
            else
            {
                // 튜토리얼 했을 경우 아무것도 안 함
                Debug.Log("이미 튜토리얼을 완료한 플레이어입니다.");
            }
        }
    }

    // New Input System의 Player Input 컴포넌트에서 호출되는 이동 이벤트 함수
    public void OnMove(InputValue value)
    {
        input = value.Get<Vector2>();
        velocity = input.normalized * moveSpeed;

        // 이동 입력이 들어왔을 때 방향에 맞는 스프라이트 배열로 교체
        if (input.sqrMagnitude > 0.01f)
        {
            // 상하 방향보다 좌우 입력 값이 더 클 때 (좌우 이동 대각선 처리)
            if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
            {
                if (input.x > 0)
                {
                    ChangeSprites(spriteRight);
                }
                else
                {
                    ChangeSprites(spriteLeft);
                }
            }
            // 좌우 방향보다 상하 입력 값이 더 크거나 같을 때 (상하 이동)
            else
            {
                if (input.y > 0)
                {
                    ChangeSprites(spriteUp);
                }
                else
                {
                    ChangeSprites(spriteDown);
                }
            }
        }
    }

    private void Update()
    {
        // 멈춰있을 때는 애니메이션을 재생하지 않고 0번째 프레임(정지 모션)으로 고정
        if (input.sqrMagnitude <= 0.01f)
        {
            frameIndex = 0;
            if (currentSprites != null && currentSprites.Length > 0)
            {
                sr.sprite = currentSprites[frameIndex];
            }
            return;
        }

        // 움직이고 있을 때 실시간 타이머 기반 애니메이션 프레임 전환
        timer += Time.deltaTime;

        if (timer >= frameTime)
        {
            timer = 0f;
            frameIndex++;

            // 배열 크기를 벗어나면 첫 프레임으로 되돌림 (루프 재생)
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
        // 물리 기반의 안정적인 위치 이동 계산
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }

    // 방향 전환 시 애니메이션 상태를 초기화해주는 함수
    private void ChangeSprites(Sprite[] newSprites)
    {
        if (currentSprites == newSprites || newSprites == null || newSprites.Length == 0)
            return;

        currentSprites = newSprites;
        frameIndex = 0;
        timer = 0f;
        sr.sprite = currentSprites[frameIndex];
    }

    // 5. 적(Enemy) 충돌 및 게임오버 처리 로직 결합
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.GameOver();
            }
            else
            {
                Debug.LogError("GameManager 인스턴스를 찾을 수 없어 GameOver를 호출할 수 없습니다.");
            }
        }
    }
}
