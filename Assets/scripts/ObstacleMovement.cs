using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMovement : MonoBehaviour
{
    [HideInInspector]
    public float speed; // السرعة التي يستقبلها من الـ Spawner

    void Update()
    {
        // التحرك للأسفل باستمرار
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        // حذف العقبة إذا خرجت من الشاشة من الأسفل لتوفير الذاكرة
        if (transform.position.y < -10f)
        {
            Destroy(gameObject);
        }
    }
}