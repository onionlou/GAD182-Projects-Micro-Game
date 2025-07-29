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
    [SerializeField] private Transform visualChild; // Drag your sprite child here in Inspector
    
    [Header("Audio Clips")]
    private AudioSource audioSource;
    public AudioClip launchSound;
    public AudioClip hitSound;

    private Vector2 direction;
    private bool hasDirection = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource != null && launchSound != null)
        {
            audioSource.PlayOneShot(launchSound);
        }
    }
    private void Update()
    {
        if (!hasDirection) return;
        transform.Translate(direction * speed * Time.deltaTime);
    }

    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized;
        hasDirection = true;

        // 🔄 Rotate the visual child from default facing (down) to match direction
        float angle = Vector2.SignedAngle(Vector2.down, direction);
        visualChild.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            if (audioSource != null && hitSound != null)
            {
                AudioSource.PlayClipAtPoint(hitSound, transform.position);
            }
            GameManager1.Instance.GameOverOrWin(other.tag);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}