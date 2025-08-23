using UnityEngine;

public class BH_Item : MonoBehaviour
{
    public int healAmount = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        BH_Player player = collision.GetComponent<BH_Player>();
        if (player != null)
        {
            player.health += healAmount;
            Debug.Log("[BH] Player healed! Health: " + player.health);
            Destroy(gameObject);
        }
    }
}