using UnityEngine;

// نيزك الكيبورد: بينزل من فوق وبيقتل أي نيزك أو عدو بيلمسه (مبيأذيش اللاعب)
public class AbilityMeteor : MonoBehaviour
{
    private float fallSpeed;
    private float lifetime = 5f;
    private float timer;
    private float relX;   // مكانه الأفقي بالنسبة للكاميرا (عشان ميتأخرش والكاميرا ماشية)
    private float y;
    private float z;
    private Camera cam;
    private SpriteRenderer sr;

    public void Init(Sprite sprite, float size, float fallSpeed, float startRelX)
    {
        this.fallSpeed = fallSpeed;

        cam = Camera.main;
        relX = startRelX;
        y = transform.position.y;
        z = transform.position.z;

        sr = gameObject.AddComponent<SpriteRenderer>();
        sr.sprite = sprite != null ? sprite : AbilityUtils.Circle();
        if (sprite == null) sr.color = new Color(1f, 0.5f, 0.1f, 1f);
        sr.sortingOrder = 5;

        float spriteHeight = sr.sprite.bounds.size.y;
        transform.localScale = Vector3.one * (size / spriteHeight);
    }

    void LateUpdate()
    {
        timer += Time.deltaTime;
        if (timer >= lifetime)
        {
            Destroy(gameObject);
            return;
        }

        y -= fallSpeed * Time.deltaTime;

        if (cam != null)
            transform.position = new Vector3(cam.transform.position.x + relX, y, z);
        else
            transform.position = new Vector3(transform.position.x, y, z);

        // لو نزل تحت الشاشة خلاص
        if (cam != null && y < cam.transform.position.y - cam.orthographicSize - 3f)
        {
            Destroy(gameObject);
            return;
        }

        Collider2D[] hits = Physics2D.OverlapBoxAll(sr.bounds.center, sr.bounds.size, 0f);
        foreach (Collider2D hit in hits)
        {
            AbilityUtils.TryKill(hit);
        }
    }
}