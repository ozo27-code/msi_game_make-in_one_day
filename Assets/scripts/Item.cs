using UnityEngine;

public enum ItemType
{
    Laptop,
    Headphones,
    Keyboard
}

// الأيتم اللي بيظهر في اللعبة ولما اللاعب يلمسه بياخده
// الـ Prefab لازم يكون عليه SpriteRenderer و Collider2D (Is Trigger) و السكريبت ده
public class Item : MonoBehaviour
{
    public ItemType type;

    public void Setup(ItemType newType, Sprite sprite, float size)
    {
        type = newType;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null) sr = gameObject.AddComponent<SpriteRenderer>();

        if (sprite != null) sr.sprite = sprite;
        if (sr.sprite == null) sr.sprite = AbilityUtils.Circle(); // عشان يبان حتى لو مفيش صورة

        sr.enabled = true;
        sr.color = Color.white;
        sr.sortingOrder = 10; // فوق الخلفية

        // حجم ثابت وواضح مهما كان حجم الصورة
        float h = sr.sprite.bounds.size.y;
        if (h > 0f) transform.localScale = Vector3.one * (size / h);

        // لازم يكون فيه Collider بوضع Trigger عشان اللاعب ياخده
        Collider2D col = GetComponent<Collider2D>();
        if (col == null) col = gameObject.AddComponent<CircleCollider2D>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        PlayerAbilities abilities = collision.GetComponentInParent<PlayerAbilities>();
        if (abilities == null) return;

        abilities.Collect(type);
        Destroy(gameObject);
    }
}