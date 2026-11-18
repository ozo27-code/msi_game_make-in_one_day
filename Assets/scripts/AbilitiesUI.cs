using UnityEngine;
using UnityEngine.UI;

// الـ UI بتاع الأيتمز (بالـ Canvas العادي): بيحدّث العدد اللي معاك من كل أيتم
// اسحب الـ Images والـ Texts من الـ Canvas في الخانات
public class AbilitiesUI : MonoBehaviour
{
    [Header("Icons (Image)")]
    public Image laptopIcon;
    public Image headphonesIcon;
    public Image keyboardIcon;

    [Header("Counts (Text)")]
    public Text laptopText;
    public Text headphonesText;
    public Text keyboardText;

    [Header("Colors")]
    public Color emptyColor = new Color(1f, 1f, 1f, 0.3f);
    public Color haveColor = Color.white;
    public Color activeColor = new Color(0.4f, 1f, 0.4f, 1f);

    private PlayerAbilities abilities;

    void Update()
    {
        if (abilities == null) abilities = FindFirstObjectByType<PlayerAbilities>();
        bool alive = abilities != null;

        // اللابتوب: لو شغال بيوري كام ضربة فاضلة
        int laptopCount = alive ? abilities.LaptopCount : 0;
        bool laptopOn = alive && abilities.LaptopActive;
        string laptopLabel = "x" + laptopCount;
        if (laptopOn) laptopLabel += "  ON (" + abilities.ShieldHitsLeft + ")";
        Refresh(laptopIcon, laptopText, laptopLabel, laptopCount > 0, laptopOn);

        int headphonesCount = alive ? abilities.HeadphonesCount : 0;
        Refresh(headphonesIcon, headphonesText, "x" + headphonesCount, headphonesCount > 0, false);

        int keyboardCount = alive ? abilities.KeyboardCount : 0;
        Refresh(keyboardIcon, keyboardText, "x" + keyboardCount, keyboardCount > 0, false);
    }

    void Refresh(Image icon, Text label, string text, bool have, bool active)
    {
        if (label != null)
        {
            label.text = text;
            label.color = active ? activeColor : (have ? Color.white : new Color(1f, 1f, 1f, 0.5f));
        }
        if (icon != null)
        {
            icon.color = active ? activeColor : (have ? haveColor : emptyColor);
        }
    }
}