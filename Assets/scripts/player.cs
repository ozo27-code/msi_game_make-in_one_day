using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // إذا كنت تستخدم نظام الإدخال الجديد، أو يمكنك حذفه لو تستخدم النظام القديم

public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    public float playerSpeed;
    private Rigidbody2D rb;
    private Vector2 playerDirection;

    [Header("Health Settings")]
    public int maxHealth = 2; // عدد الضربات المسموح بها (ضربتين)
    private int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth; // تعيين الهيلث بالحد الأقصى عند بداية اللعبة
    }

    // Update is called once per frame
    void Update()
    {
        // قراءة الحركة (طريقة الإدخال القديمة أو الجديدة حسب إعدادات مشروعك)
        float directionY = Input.GetAxisRaw("Vertical");
        playerDirection = new Vector2(0, directionY).normalized;
    }

    void FixedUpdate()
    {
        // تطبيق الحركة على اللاعب
        rb.linearVelocity = new Vector2(0, playerDirection.y * playerSpeed);
    }

    // دالة لاستقبال الضرر وتحديد الموت بعد ضربتين
    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log("Player Health remaining: " + currentHealth);

        // إذا نفدت الأرواح بالكامل (بعد الخبطة الثانية)
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player Died!");

        // تدمير اللاعب عند الموت
        Destroy(gameObject);

        // يمكنك هنا لاحقاً استدعاء شاشة Game Over أو إعادة تحميل اللعبة
    }
}