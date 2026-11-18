using UnityEngine;

// بيطلّع أيتم واحد (لابتوب / سماعة / كيبورد) كل فترة طويلة، مش على طول
public class ItemSpawner : MonoBehaviour
{
    [Header("Item Settings")]
    public GameObject itemPrefab;      // Prefab عليه Item + SpriteRenderer + Collider2D (Is Trigger)
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
    public float edgeMargin = 1.5f;        // بعد كام عن حافة الشاشة فوق وتحت (عشان دايماً يبان)
    public float offsetOutsideScreen = 1f; // بيظهر برا الشاشة يمين بالمسافة دي

    private float nextSpawnTime;
    private PlayerAbilities player;

    void Start()
    {
        Debug.Log("ItemSpawner شغال. أول أيتم بعد " + firstDelay + " ثانية.");
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
        if (itemPrefab == null)
        {
            Debug.LogWarning("ItemSpawner: خانة Item Prefab فاضية!", this);
            return;
        }

        if (player == null) player = FindFirstObjectByType<PlayerAbilities>();
        if (player == null)
        {
            Debug.LogWarning("ItemSpawner: مش لاقي سكريبت PlayerAbilities! ضيفه على الـ Player.", this);
            return;
        }

        Camera cam = Camera.main;
        if (cam == null)
        {
            Debug.LogWarning("ItemSpawner: مش لاقي الكاميرا! حط Tag = MainCamera على الكاميرا.", this);
            return;
        }

        // نختار نوع من اللي اللاعب لسه معاهوش
        ItemType[] all = { ItemType.Laptop, ItemType.Headphones, ItemType.Keyboard };
        ItemType[] available = new ItemType[3];
        int count = 0;
        foreach (ItemType t in all)
        {
            if (!player.Has(t)) available[count++] = t;
        }
        if (count == 0)
        {
            Debug.Log("ItemSpawner: اللاعب معاه التلاتة أصلاً.");
            return;
        }

        ItemType chosen = available[Random.Range(0, count)];

        float halfWidth = cam.orthographicSize * cam.aspect;
        float x = cam.transform.position.x + halfWidth + offsetOutsideScreen;
        float halfHeight = cam.orthographicSize;
        float y = cam.transform.position.y + Random.Range(-(halfHeight - edgeMargin), halfHeight - edgeMargin);

        GameObject obj = Instantiate(itemPrefab, new Vector3(x, y, 0f), Quaternion.identity);

        Sprite sprite = laptopSprite;
        if (chosen == ItemType.Headphones) sprite = headphonesSprite;
        else if (chosen == ItemType.Keyboard) sprite = keyboardSprite;

        Item item = obj.GetComponent<Item>();
        if (item != null) item.Setup(chosen, sprite, itemSize);
        else Debug.LogWarning("ItemSpawner: الـ Prefab مفيهوش سكريبت Item!", this);

        Debug.Log("ItemSpawner: ظهر " + chosen + " في المكان " + obj.transform.position);

        Destroy(obj, itemLifetime);
    }
}