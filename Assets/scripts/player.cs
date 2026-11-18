using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // مكتبة نظام الإدخال الجديد

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
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        // التأكد من أن لوحة المفاتيح متصلة لتجنب أي أخطاء
        if (Keyboard.current == null) return;

        float directionY = 0f;

        // قراءة أزرار الحركة عبر الـ New Input System (W, S أو السهم العلوي والسفلي)
        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
        {
            directionY = 1f;
        }
        else if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
        {
            directionY = -1f;
        }

        playerDirection = new Vector2(0, directionY).normalized;
    }

    void FixedUpdate()
    {
        // تطبيق الحركة على الـ Rigidbody2D
        rb.linearVelocity = new Vector2(0, playerDirection.y * playerSpeed);
    }

    // دالة استقبال الضرر عند الاصطدام بالنيزك
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

        // تفعيل شاشة Game Over فوراً
        GameOver gameOverScript = FindObjectOfType<GameOver>();
        if (gameOverScript != null)
        {
            gameOverScript.TriggerGameOver();
        }

        // تدمير اللاعب
        Destroy(gameObject);
    }
}