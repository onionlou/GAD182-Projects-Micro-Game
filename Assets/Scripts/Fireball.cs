using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls movement and collision of the fireball projectile.
/// Direction is set externally via SetDirection().
/// </summary>
public class Fireball : MonoBehaviour
{
    [SerializeField] private float speed = 5f;

    private Vector2 direction;
    private bool hasDirection = false;

    private void Update()
    {
        if (!hasDirection) return;
        transform.Translate(direction * speed * Time.deltaTime);
    }

    /// <summary>
    /// Set the direction the fireball should move in.
    /// </summary>
    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized;
        hasDirection = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Goblin"))
        {
            GameManager1.Instance.GameOverOrWin(other.tag);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
