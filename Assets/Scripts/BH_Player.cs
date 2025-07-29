using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BH_Player : MonoBehaviour
{
    Vector3 MousePos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        transform.position = new Vector3(MousePos.x, -3.5f, 0); 
    }
}
