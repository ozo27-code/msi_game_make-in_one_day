using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Obstacle touched: " + collision.name + " | Tag: " + collision.tag);

        if (collision.tag == "Border")
        {
            Destroy(this.gameObject);
        }
        else if (collision.tag == "Player")
        {
            // البحث عن سكربت اللاعب وخصم حياة منه
            Player playerScript = collision.GetComponent<Player>();
            if (playerScript != null)
            {
                playerScript.TakeDamage(1); // خصم ضربة واحدة
            }

            // تدمير النيزك بعد ما يخبط اللاعب
            Destroy(this.gameObject);
        }
    }
}