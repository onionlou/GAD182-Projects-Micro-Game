using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BH_Item : MonoBehaviour
{

    public BoxCollider2D PlayerCollider;




    private void Start()
    {
        PlayerCollider = GameObject.Find("Player").GetComponent<BoxCollider2D>();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider == PlayerCollider)
        {
            Debug.Log("Hit");

        }
        else
        {
            Destroy(this.gameObject);
        }

        
    }
}
