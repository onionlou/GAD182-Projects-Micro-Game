using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BH_Player : MonoBehaviour
{

    Animator animator;
    Rigidbody2D RB;
    Vector3 MousePos;
    [Tooltip("adds deadzone to stop RB.AddForce from teaking")] 
    public float ThresholdRange = 0.5f;
    public float Speed = 15f;
    public bool IsDead;



    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        RB = GetComponent<Rigidbody2D>();
        GameEvents.current.OnPlayerHit += PlayerHit;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsDead)
        {
            MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            animator.SetFloat("Movement", RB.velocity.x);

            if (transform.position.x < MousePos.x + ThresholdRange)
            {
                RB.AddForce(new Vector2(Speed, 0));
            }


            if (transform.position.x > MousePos.x + -ThresholdRange)
            {
                RB.AddForce(new Vector2(-Speed, 0));
            }

        }
        
    }

    public void PlayerHit()
    {
        IsDead = true;

        RB.constraints = RigidbodyConstraints2D.None;
        RB.drag = 1;
        RB.AddForce(new Vector2(0, 1000));

        
        Debug.Log("player dead");


    }
}
