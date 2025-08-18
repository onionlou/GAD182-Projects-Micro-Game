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

    

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        RB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
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
