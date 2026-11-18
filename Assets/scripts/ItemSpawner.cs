using UnityEngine;

// بيطلّع أيتم واحد (لابتوب / سماعة / كيبورد) كل فترة، مش على طول
// مفيش Prefab: الأيتم بيتبني بالكود وبياخد نفس Layer واللاعب عشان يظهر
public class ItemSpawner : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite laptopSprite;
    public Sprite headphonesSprite;
    public Sprite keyboardSprite;

    [Header("Timing (بالثواني)")]
    public float firstDelay = 10f;     // أول أيتم يظهر بعد قد إيه من بداية اللعبة
    public float minTimeBetween = 15f;
    public float maxTimeBetween = 25f;
    public float itemLifetime = 15f;   // يتمسح لو اللاعب مأخدوش

    [Header("Spawn Position")]
    public float itemSize = 1.5f;          // حجم الأيتم على الشاشة (بالـ units)
    public float edgeMargin = 1.5f;        // بعد كام عن حافة الشاشة فوق وتحت
    public float offsetOutsideScreen = 1f; // بيظهر برا الشاشة يمين بالمسافة دي

    private float nextSpawnTime;
    private PlayerAbilities player;

    void Start()
    {
        nextSpawnTime = Time.time + firstDelay;
    }

    void Update()
    {
        if (Time.time < nextSpawnTime) return;

        SpawnItem();
        nextSpawnTime = Time.time + Random.Range(minTimeBetween, maxTimeBetween);
    }

    void SpawnItem()
    {
        if (player == null) player = FindFirstObjectByType<PlayerAbilities>();
        if (player == null) return; // اللاعب مات أو مفيش PlayerAbilities عليه

        Camera cam = Camera.main;
        if (cam == null)
        {
            Debug.LogWarning("ItemSpawner: مش لاقي الكاميرا! حط Tag = MainCamera على الكاميرا.", this);
            return;
        }

        // نختار نوع من اللي اللاعب لسه مش شايل أقصى عدد منه
        ItemType[] all = { ItemType.Laptop, ItemType.Headphones, ItemType.Keyboard };
        ItemType[] available = new ItemType[3];
        int count = 0;
        foreach (ItemType t in all)
        {
            if (!player.IsFull(t)) available[count++] = t;
        }
        if (count == 0) return; // شايل أقصى عدد من التلاتة

        ItemType chosen = available[Random.Range(0, count)];

        Sprite sprite = laptopSprite;
        if (chosen == ItemType.Headphones) sprite = headphonesSprite;
        else if (chosen == ItemType.Keyboard) sprite = keyboardSprite;

        float halfHeight = cam.orthographicSize;
        float halfWidth = halfHeight * cam.aspect;
        float x = cam.transform.position.x + halfWidth + offsetOutsideScreen;
        float y = cam.transform.position.y + Random.Range(-(halfHeight - edgeMargin), halfHeight - edgeMargin);

        // بناء الأيتم بالكود
        GameObject obj = new GameObject("Item_" + chosen);
        obj.transform.position = new Vector3(x, y, player.transform.position.z);

        SpriteRenderer sr = obj.AddComponent<SpriteRenderer>();

        // نفس الـ Layer والـ Sorting بتاع اللاعب (عشان يظهر زيه)
        SpriteRenderer playerSr = player.GetComponentInChildren<SpriteRenderer>();
        obj.layer = player.gameObject.layer;
        if (playerSr != null)
        {
            sr.sortingLayerID = playerSr.sortingLayerID;
            sr.sortingOrder = playerSr.sortingOrder + 5;
            sr.sharedMaterial = playerSr.sharedMaterial; // نفس الخامة (مهم في URP 2D)
        }
        else
        {
            sr.sortingOrder = 10;
        }

        Item item = obj.AddComponent<Item>();
        item.Setup(chosen, sprite, itemSize);

        Debug.Log("ItemSpawner: ظهر " + chosen + " في " + obj.transform.position + " | الصورة: " + (sprite != null ? sprite.name : "فاضية (دايرة بيضا)"));

        Destroy(obj, itemLifetime);
    }
}