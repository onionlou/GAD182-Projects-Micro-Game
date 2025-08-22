using System.Linq;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private Transform visualChild;

    [Header("Audio Clips")]
    private AudioSource audioSource;
    public AudioClip launchSound;
    public AudioClip hitSound;

    private Vector2 direction;
    private bool hasDirection = false;
    private IProjectileReactive projectileReactive;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource != null && launchSound != null)
        {
            audioSource.PlayOneShot(launchSound);
        }

        // Find the active win condition that implements IProjectileReactive
        projectileReactive = FindObjectsOfType<MonoBehaviour>()
            .OfType<IProjectileReactive>()
            .FirstOrDefault();

        if (projectileReactive == null)
        {
            Debug.LogWarning("[Fireball] No IProjectileReactive found — hits won't affect win/loss state.");
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

        if (visualChild != null)
        {
            float angle = Vector2.SignedAngle(Vector2.down, direction);
            visualChild.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"[Fireball] Triggered by: {other.name}, tag: {other.tag}");

        if (audioSource != null && hitSound != null &&
            (other.CompareTag("Player") || other.CompareTag("Enemy")))
        {
            AudioSource.PlayClipAtPoint(hitSound, transform.position);
        }

        if (projectileReactive != null &&
            (other.CompareTag("Player") || other.CompareTag("Enemy")))
        {
            Debug.Log("[Fireball] Calling OnProjectileHit...");
            projectileReactive.OnProjectileHit(other.tag);
        }

        if (other.CompareTag("Player") || other.CompareTag("Enemy") || other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}