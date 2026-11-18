using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

// حطه على الـ Player. بيشيل الأيتمز اللي اللاعب أخدها وبيشغّل قدراتها بالأزرار
public class PlayerAbilities : MonoBehaviour
{
    [Header("Keys (الأزرار)")]
    public Key laptopKey = Key.Q;
    public Key headphonesKey = Key.E;
    public Key keyboardKey = Key.R;

    [Header("Laptop (لابتوب): يستحمل ضربتين + الصحة بتتجدد + لما تموت القلوب ترجع")]
    public int laptopShieldHits = 2;     // عدد الضربات اللي يستحملها من غير ما يخسر قلب
    public float regenInterval = 3f;     // كل كام ثانية يرجع قلب

    [Header("Headphones (سماعة): موجة تقتل اللي قدامك")]
    public Sprite waveSprite;            // ultrasound
    public float waveSize = 3f;          // طول الموجة بالـ units
    public float waveSpeed = 12f;
    public float waveLifetime = 1.5f;
    public float waveStartOffsetX = 3f;  // بتبدأ قدام اللاعب بكام

    [Header("Keyboard (كيبورد): ينزل نيازك")]
    public Sprite meteorSprite;          // nezk 2
    public float meteorSize = 1.5f;
    public int meteorCount = 6;
    public float meteorInterval = 0.4f;  // الوقت بين كل نيزك والتاني
    public float meteorFallSpeed = 12f;

    [Header("Inventory (المخزون)")]
    public int maxStack = 3;             // أقصى عدد تشيله من كل أيتم

    // عدد الأيتمز اللي مع اللاعب دلوقتي (كل استخدام بينقص واحد)
    public int LaptopCount { get; private set; }
    public int HeadphonesCount { get; private set; }
    public int KeyboardCount { get; private set; }

    // حالة اللابتوب
    public bool LaptopActive { get; private set; }
    private int shieldHitsLeft;
    public int ShieldHitsLeft { get { return shieldHitsLeft; } }

    private Player player;

    void Start()
    {
        player = GetComponent<Player>();
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current[laptopKey].wasPressedThisFrame && LaptopCount > 0 && !LaptopActive)
        {
            LaptopCount--;
            if (AudioManager.Instance != null) AudioManager.Instance.PlayLaptop();
            StartCoroutine(LaptopRoutine());
        }

        if (Keyboard.current[headphonesKey].wasPressedThisFrame && HeadphonesCount > 0)
        {
            HeadphonesCount--;
            if (AudioManager.Instance != null) AudioManager.Instance.PlayWave();
            FireWave();
        }

        if (Keyboard.current[keyboardKey].wasPressedThisFrame && KeyboardCount > 0)
        {
            KeyboardCount--;
            if (AudioManager.Instance != null) AudioManager.Instance.PlayMeteor();
            StartCoroutine(MeteorRoutine());
        }
    }

    // ===== الأيتمز =====

    // true لو اللاعب شايل أقصى عدد من الأيتم ده (السبونر مش بيطلّعه تاني)
    public bool IsFull(ItemType type)
    {
        switch (type)
        {
            case ItemType.Laptop: return LaptopCount >= maxStack;
            case ItemType.Headphones: return HeadphonesCount >= maxStack;
            default: return KeyboardCount >= maxStack;
        }
    }

    public void Collect(ItemType type)
    {
        switch (type)
        {
            case ItemType.Laptop: LaptopCount = Mathf.Min(LaptopCount + 1, maxStack); break;
            case ItemType.Headphones: HeadphonesCount = Mathf.Min(HeadphonesCount + 1, maxStack); break;
            case ItemType.Keyboard: KeyboardCount = Mathf.Min(KeyboardCount + 1, maxStack); break;
        }
        Debug.Log("Collected: " + type);
    }

    // ===== اللابتوب =====

    // بيتنادي من Player.TakeDamage: لو رجّع true يبقى الضربة اتمتصت
    public bool TryAbsorbHit()
    {
        if (LaptopActive && shieldHitsLeft > 0)
        {
            shieldHitsLeft--;
            return true;
        }
        return false;
    }

    // بيتنادي من Player لما الصحة توصل صفر: لو اللابتوب شغال يرجّع القلوب (مرة واحدة) واللابتوب يخلص
    public bool TryRevive()
    {
        if (!LaptopActive) return false;

        LaptopActive = false;
        shieldHitsLeft = 0;
        return true;
    }

    IEnumerator LaptopRoutine()
    {
        LaptopActive = true;
        shieldHitsLeft = laptopShieldHits;

        float regenTimer = 0f;

        // شغال لحد ما اللاعب يموت (TryRevive بتوقفه)
        while (LaptopActive)
        {
            regenTimer += Time.deltaTime;

            if (regenTimer >= regenInterval)
            {
                regenTimer = 0f;
                if (player != null) player.Heal(1);
            }

            yield return null;
        }
    }

    // بيخلي الحاجات اللي بتتبني بالكود تستخدم نفس خامة وLayer اللاعب عشان تظهر صح
    void ApplyPlayerLook(GameObject obj)
    {
        SpriteRenderer playerSr = GetComponentInChildren<SpriteRenderer>();
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        if (playerSr == null || sr == null) return;

        sr.sharedMaterial = playerSr.sharedMaterial;
        sr.sortingLayerID = playerSr.sortingLayerID;
        sr.sortingOrder = playerSr.sortingOrder + 5;
        obj.layer = gameObject.layer;
    }

    // ===== السماعة =====

    void FireWave()
    {
        GameObject waveObj = new GameObject("Wave");
        waveObj.transform.position = transform.position + Vector3.right * waveStartOffsetX;

        // مكان البداية بالنسبة للكاميرا (عشان الموجة متروحش لورا)
        Camera cam = Camera.main;
        float startRelX = cam != null ? waveObj.transform.position.x - cam.transform.position.x : 0f;

        AbilityWave wave = waveObj.AddComponent<AbilityWave>();
        wave.Init(waveSprite, waveSize, waveSpeed, waveLifetime, startRelX);
        ApplyPlayerLook(waveObj);
    }

    // ===== الكيبورد =====

    IEnumerator MeteorRoutine()
    {
        Camera cam = Camera.main;
        if (cam == null) yield break;

        for (int i = 0; i < meteorCount; i++)
        {
            float halfHeight = cam.orthographicSize;
            float halfWidth = halfHeight * cam.aspect;

            // النيازك بتنزل في النص اللي قدام اللاعب من الشاشة
            float minX = transform.position.x + 1f;
            float maxX = cam.transform.position.x + halfWidth;
            if (minX >= maxX) minX = cam.transform.position.x - halfWidth;

            float x = Random.Range(minX, maxX);
            float y = cam.transform.position.y + halfHeight + 1f;

            GameObject meteorObj = new GameObject("KeyboardMeteor");
            meteorObj.transform.position = new Vector3(x, y, 0f);

            AbilityMeteor meteor = meteorObj.AddComponent<AbilityMeteor>();
            meteor.Init(meteorSprite, meteorSize, meteorFallSpeed, x - cam.transform.position.x);
            ApplyPlayerLook(meteorObj);

            yield return new WaitForSeconds(meteorInterval);
        }
    }
}