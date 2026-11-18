using UnityEngine;
using UnityEngine.UI;

// بيظهر فوق الشاشة أيقونات اللابتوب والسماعة والكيبورد:
// باهتة = مش معاك، واضحة = معاك، خضرا = اللابتوب شغال دلوقتي
public class AbilitiesUI : MonoBehaviour
{
    [Header("Icons (Image)")]
    public Image laptopIcon;
    public Image headphonesIcon;
    public Image keyboardIcon;

    [Header("Colors")]
    public Color notOwnedColor = new Color(1f, 1f, 1f, 0.25f);
    public Color ownedColor = Color.white;
    public Color activeColor = new Color(0.4f, 1f, 0.4f, 1f);

    private PlayerAbilities abilities;

    void Update()
    {
        if (abilities == null) abilities = FindFirstObjectByType<PlayerAbilities>();

        bool alive = abilities != null;

        SetColor(laptopIcon, alive && abilities.LaptopActive ? activeColor
                           : alive && abilities.HasLaptop ? ownedColor : notOwnedColor);
        SetColor(headphonesIcon, alive && abilities.HasHeadphones ? ownedColor : notOwnedColor);
        SetColor(keyboardIcon, alive && abilities.HasKeyboard ? ownedColor : notOwnedColor);
    }

    void SetColor(Image img, Color c)
    {
        if (img != null) img.color = c;
    }
}