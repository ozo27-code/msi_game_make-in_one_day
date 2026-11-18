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

    public void Setup(ItemType newType, Sprite sprite)
    {
        type = newType;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null && sprite != null) sr.sprite = sprite;
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