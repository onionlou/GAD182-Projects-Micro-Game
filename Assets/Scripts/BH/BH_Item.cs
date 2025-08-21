using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BH_Item : MonoBehaviour
{

    public BoxCollider2D PlayerCollider;
    public BoxCollider2D DeletePlane;



    private void Start()
    {
        PlayerCollider = GameObject.Find("Player").GetComponent<BoxCollider2D>();
        DeletePlane = GameObject.Find("Delete plane").GetComponent<BoxCollider2D>();
        
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider == PlayerCollider)
        {
            GameEvents.current.PlayerHit();

        }
        if (collision.collider == DeletePlane)
        {
            Destroy(this.gameObject);
        }

        
    }
}
