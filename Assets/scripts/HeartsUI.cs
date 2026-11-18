using UnityEngine;
using UnityEngine.UI;

public class HeartsUI : MonoBehaviour
{
    public Image[] hearts; // اسحب القلوب هنا بالترتيب

    public void UpdateHearts(int currentHealth)
    {
        Debug.Log("UpdateHearts called. Health = " + currentHealth + " | Hearts slots = " + hearts.Length);

        for (int i = 0; i < hearts.Length; i++)
        {
            if (hearts[i] == null)
            {
                Debug.LogWarning("Heart slot " + i + " is EMPTY");
                continue;
            }

            hearts[i].enabled = i < currentHealth;
        }
    }
}