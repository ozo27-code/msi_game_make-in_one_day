using UnityEngine;

// حطه على أي عدو (الفونكس / المجما ولف / العقرب) عشان موجة السماعة ونيازك الكيبورد تقتله
public class Killable : MonoBehaviour
{
    public void Kill()
    {
        Destroy(gameObject);
    }
}

// أدوات مشتركة للقدرات
public static class AbilityUtils
{
    // بتقتل أي نيزك (عليه Obstacle) أو أي عدو (عليه Killable). بترجع true لو قتلت حاجة
    public static bool TryKill(Collider2D col)
    {
        Killable killable = col.GetComponentInParent<Killable>();
        if (killable != null)
        {
            killable.Kill();
            return true;
        }

        Obstacle obstacle = col.GetComponentInParent<Obstacle>();
        if (obstacle != null)
        {
            Object.Destroy(obstacle.gameObject);
            return true;
        }

        return false;
    }

    private static Sprite circle;

    // دايرة بيضا بتتعمل بالكود، بتستخدم لو مفيش صورة متحطة
    public static Sprite Circle()
    {
        if (circle != null) return circle;

        int size = 64;
        Texture2D tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        float r = size / 2f;
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float d = Vector2.Distance(new Vector2(x + 0.5f, y + 0.5f), new Vector2(r, r));
                tex.SetPixel(x, y, d <= r ? Color.white : Color.clear);
            }
        }
        tex.Apply();
        circle = Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), size);
        return circle;
    }
}