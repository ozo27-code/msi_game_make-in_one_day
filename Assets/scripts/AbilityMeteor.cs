using UnityEngine;

// نيزك الكيبورد: بينزل من فوق وبيقتل أي نيزك أو عدو بيلمسه (مبيأذيش اللاعب)
public class AbilityMeteor : MonoBehaviour
{
    private float fallSpeed;
    private float lifetime = 5f;
    private float timer;
    private SpriteRenderer sr;

    public void Init(Sprite sprite, float size, float fallSpeed)
    {
        this.fallSpeed = fallSpeed;

        sr = gameObject.AddComponent<SpriteRenderer>();
        sr.sprite = sprite != null ? sprite : AbilityUtils.Circle();
        if (sprite == null) sr.color = new Color(1f, 0.5f, 0.1f, 1f);
        sr.sortingOrder = 5;

        float spriteHeight = sr.sprite.bounds.size.y;
        transform.localScale = Vector3.one * (size / spriteHeight);
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= lifetime)
        {
            Destroy(gameObject);
            return;
        }

        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime, Space.World);

        Collider2D[] hits = Physics2D.OverlapBoxAll(sr.bounds.center, sr.bounds.size, 0f);
        foreach (Collider2D hit in hits)
        {
            AbilityUtils.TryKill(hit);
        }
    }
}