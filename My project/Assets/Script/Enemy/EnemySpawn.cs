using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("적 프리팹 및 데이터")]
    [SerializeField] private GameObject enemyPrefab;       // 생성할 적 프리팹
    [SerializeField] private EnemyStatus enemyData;         // 이미지로 만드신 ScriptableObject 데이터

    [Header("스폰 속도 설정")]
    [SerializeField] private float spawnInterval = 3f;      // 몇 초마다 적을 만들 것인가
    private float timer = 0f;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    private void SpawnEnemy()
    {
        if (enemyPrefab == null) return;

        // 현재 스폰 지점의 위치에 적 생성
        GameObject newEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);

        // [참고] 생성된 적 스크립트에 ScriptableObject 데이터를 주입하고 싶다면 아래처럼 활용합니다.
        // EnemyController enemyScript = newEnemy.GetComponent<EnemyController>();
        // if (enemyScript != null) enemyScript.Init(enemyData);
    }
}
