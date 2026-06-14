using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("스폰 포인트 프리팹")]
    [SerializeField] private GameObject enemySpawnPrefab; // EnemySpawner 컴포넌트가 붙은 프리팹

    [Header("스폰 포인트 증식 주기")]
    [SerializeField] private float pointGenerationInterval = 10f; // 몇 초마다 스폰 구역을 늘릴 것인가
    private float generationTimer = 0f;

    [Header("맵 생성 범위 설정")]
    // 라이브러리 맵 안쪽 영역의 좌표 범위를 적절히 지정해 줍니다.
    [SerializeField] private float minX = -5f;
    [SerializeField] private float maxX = 5f;
    [SerializeField] private float minY = -3f;
    [SerializeField] private float maxY = 3f;

    private void Start()
    {
        // 게임 시작 시 최초로 1개의 스폰 구역은 깔고 시작합니다.
        GenerateNewSpawnPoint();
    }

    private void Update()
    {
        generationTimer += Time.deltaTime;

        // 시간이 흐를수록 스폰 지점 자체를 추가 생성
        if (generationTimer >= pointGenerationInterval)
        {
            GenerateNewSpawnPoint();
            generationTimer = 0f;
        }
    }

    private void GenerateNewSpawnPoint()
    {
        if (enemySpawnPrefab == null) return;

        // 설정한 범위 내에서 랜덤한 격자/좌표 계산
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);
        Vector3 spawnPosition = new Vector3(randomX, randomY, 0f);

        // 새로운 스폰 포인트 배치
        GameObject newPoint = Instantiate(enemySpawnPrefab, spawnPosition, Quaternion.identity);
        Debug.Log($"새로운 에너미 스폰 구역이 {spawnPosition} 위치에 생성되었습니다!");
    }
}