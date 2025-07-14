using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Allows the player to fire a fireball by clicking.
/// </summary>
public class PlayerCannonShooter : MonoBehaviour
{
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private Transform firePoint;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject fireball = Instantiate(fireballPrefab, firePoint.position, Quaternion.identity);
            fireball.GetComponent<Fireball>().SetDirection(Vector2.up);
        }
    }
}



