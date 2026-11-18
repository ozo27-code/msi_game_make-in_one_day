using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [Header("Coin Settings")]
    public GameObject coinPrefab;

    [Header("Spawn Position")]
    public float minX = 8f;   // بعيد قدام الكاميرا شوية
    public float maxX = 12f;
    public float minY = -3f;  // جوه مجال حركة اللاعب
    public float maxY = 3f;

    [Header("Spawn Settings")]
    public float timeBetweenSpawn = 3f;
    public float coinLifetime = 15f; // يتمسح بعد المدة دي لو متاخدش

    private float spawnTime;

    void Update()
    {
        if (Time.time >= spawnTime)
        {
            SpawnCoin();
            spawnTime = Time.time + timeBetweenSpawn;
        }
    }

    void SpawnCoin()
    {
        if (coinPrefab == null)
        {
            Debug.LogWarning("CoinSpawner على " + gameObject.name + ": خانة Coin Prefab فاضية!", this);
            return;
        }

        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);

        Vector3 position = new Vector3(transform.position.x + randomX, randomY, 0f);

        GameObject newCoin = Instantiate(coinPrefab, position, Quaternion.identity);

        // الكوين لازم يفضل ثابت: نوقف أي سكريبت حركة لو موجود
        ObstacleMovement movement = newCoin.GetComponent<ObstacleMovement>();
        if (movement != null) movement.enabled = false;

        Destroy(newCoin, coinLifetime);
    }
}