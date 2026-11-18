using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObstacles : MonoBehaviour
{
    public GameObject obstacle;
    public float maxX;
    public float minX;

    [Header("Y Spawn Range (منين تبدأ تظهر)")]
    public float maxY; // أقصى ارتفاع من فوق
    public float minY; // أقل ارتفاع (مثلاً في المنتصف أو أسفل شوية)

    public float timeBetweenSpawn;

    [Header("Fall Settings")]
    public float obstacleFallSpeed = 5f; // سرعة السقوط

    private float spawnTime;

    void Update()
    {
        if (Time.time > spawnTime)
        {
            Spawn();
            spawnTime = Time.time + timeBetweenSpawn;
        }
    }

    void Spawn()
    {
        // اختيار مكان عشوائي في العرض (X) وفي الارتفاع (Y)
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);

        // بناء موقع الظهور بحيث يتحرك مع مكان الكاميرا أفقياً، ويكون عشوائياً رأسياً
        Vector3 spawnPosition = new Vector3(transform.position.x + randomX, randomY, 0f);

        // إنشاء العقبة
        GameObject newObstacle = Instantiate(obstacle, spawnPosition, transform.rotation);

        // إعطاء السرعة للعقبة لتبدأ في السقوط
        ObstacleMovement moveScript = newObstacle.GetComponent<ObstacleMovement>();
        if (moveScript != null)
        {
            moveScript.speed = obstacleFallSpeed;
        }
    }
}
