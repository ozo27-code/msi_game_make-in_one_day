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

    [Header("Laptop (لابتوب): يستحمل ضربتين + الصحة بتتجدد")]
    public float laptopDuration = 10f;   // مدة القدرة
    public int laptopShieldHits = 2;     // عدد الضربات اللي يستحملها من غير ما يخسر قلب
    public float regenInterval = 3f;     // كل كام ثانية يرجع قلب

    [Header("Headphones (سماعة): موجة تقتل اللي قدامك")]
    public Sprite waveSprite;            // ultrasound
    public float waveSize = 3f;          // طول الموجة بالـ units
    public float waveSpeed = 12f;
    public float waveLifetime = 1.5f;
    public float waveStartOffsetX = 1f;  // بتبدأ قدام اللاعب بكام

    [Header("Keyboard (كيبورد): ينزل نيازك")]
    public Sprite meteorSprite;          // nezk 2
    public float meteorSize = 1.5f;
    public int meteorCount = 6;
    public float meteorInterval = 0.4f;  // الوقت بين كل نيزك والتاني
    public float meteorFallSpeed = 12f;

    // الأيتمز اللي مع اللاعب دلوقتي (كل أيتم استخدام واحد)
    public bool HasLaptop { get; private set; }
    public bool HasHeadphones { get; private set; }
    public bool HasKeyboard { get; private set; }

    // حالة اللابتوب
    public bool LaptopActive { get; private set; }
    private int shieldHitsLeft;

    private Player player;

    void Start()
    {
        player = GetComponent<Player>();
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current[laptopKey].wasPressedThisFrame && HasLaptop && !LaptopActive)
        {
            HasLaptop = false;
            StartCoroutine(LaptopRoutine());
        }

        if (Keyboard.current[headphonesKey].wasPressedThisFrame && HasHeadphones)
        {
            HasHeadphones = false;
            FireWave();
        }

        if (Keyboard.current[keyboardKey].wasPressedThisFrame && HasKeyboard)
        {
            HasKeyboard = false;
            StartCoroutine(MeteorRoutine());
        }
    }

    // ===== الأيتمز =====

    public bool Has(ItemType type)
    {
        switch (type)
        {
            case ItemType.Laptop: return HasLaptop || LaptopActive;
            case ItemType.Headphones: return HasHeadphones;
            default: return HasKeyboard;
        }
    }

    public void Collect(ItemType type)
    {
        switch (type)
        {
            case ItemType.Laptop: HasLaptop = true; break;
            case ItemType.Headphones: HasHeadphones = true; break;
            case ItemType.Keyboard: HasKeyboard = true; break;
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

    IEnumerator LaptopRoutine()
    {
        LaptopActive = true;
        shieldHitsLeft = laptopShieldHits;

        float elapsed = 0f;
        float regenTimer = 0f;

        while (elapsed < laptopDuration)
        {
            elapsed += Time.deltaTime;
            regenTimer += Time.deltaTime;

            if (regenTimer >= regenInterval)
            {
                regenTimer = 0f;
                if (player != null) player.Heal(1);
            }

            yield return null;
        }

        LaptopActive = false;
        shieldHitsLeft = 0;
    }

    // ===== السماعة =====

    void FireWave()
    {
        GameObject waveObj = new GameObject("Wave");
        waveObj.transform.position = transform.position + Vector3.right * waveStartOffsetX;

        AbilityWave wave = waveObj.AddComponent<AbilityWave>();
        wave.Init(waveSprite, waveSize, waveSpeed, waveLifetime);
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
            meteor.Init(meteorSprite, meteorSize, meteorFallSpeed);

            yield return new WaitForSeconds(meteorInterval);
        }
    }
}