using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemy cannon that fires projectiles downward at fixed intervals.
/// </summary>
public class EnemyCannonShooter : MonoBehaviour
{
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float shootInterval = 1.5f;

    private void Start()
    {
        InvokeRepeating(nameof(Shoot), 1f, shootInterval);
    }

    private void Shoot()
    {
        GameObject fireball = Instantiate(fireballPrefab, firePoint.position, Quaternion.identity);
        fireball.GetComponent<Fireball>().SetDirection(Vector2.down);
    }
}


