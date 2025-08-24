using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BH_Item : MonoBehaviour
{
    private BoxCollider2D PlayerCollider;
    private BoxCollider2D DeletePlane;
    private BH_Player player;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<BH_Player>();
        PlayerCollider = player.GetComponent<BoxCollider2D>(); // added this to subscribe to the BH_Player script (communicates through the win/lose interface)
        DeletePlane = GameObject.Find("Delete plane").GetComponent<BoxCollider2D>();
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider == PlayerCollider)
        {
            player.PlayerHit();
            //GameEvents.current.PlayerHit(); <------ REMOVED as conflicts with Master Scene game event system

        }
        if (collision.collider == DeletePlane)
        {
            Destroy(gameObject);
        }
        
    }
}
