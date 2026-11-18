using UnityEngine;

// موجة السماعة: بتمشي قدام اللاعب وبتقتل أي نيزك أو عدو بتلمسه
public class AbilityWave : MonoBehaviour
{
    private float speed;
    private float lifetime;
    private float startScale;
    private float endScale;
    private float timer;
    private SpriteRenderer sr;

    public void Init(Sprite sprite, float size, float speed, float lifetime)
    {
        this.speed = speed;
        this.lifetime = lifetime;

        sr = gameObject.AddComponent<SpriteRenderer>();
        sr.sprite = sprite != null ? sprite : AbilityUtils.Circle();
        if (sprite == null) sr.color = new Color(0.3f, 0.9f, 1f, 0.5f);
        sr.sortingOrder = 5;

        // نظبط الحجم بحيث طول الموجة = size بالـ units مهما كان حجم الصورة
        float spriteHeight = sr.sprite.bounds.size.y;
        startScale = size / spriteHeight;
        endScale = startScale * 1.5f; // الموجة بتكبر وهي ماشية
        transform.localScale = Vector3.one * startScale;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= lifetime)
        {
            Destroy(gameObject);
            return;
        }

        transform.Translate(Vector3.right * speed * Time.deltaTime, Space.World);
        transform.localScale = Vector3.one * Mathf.Lerp(startScale, endScale, timer / lifetime);

        // نقتل كل اللي جوه حدود الموجة
        Collider2D[] hits = Physics2D.OverlapBoxAll(sr.bounds.center, sr.bounds.size, 0f);
        foreach (Collider2D hit in hits)
        {
            AbilityUtils.TryKill(hit);
        }
    }
}