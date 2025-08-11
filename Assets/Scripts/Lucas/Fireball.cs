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
    private IWinCondition winCondition;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource != null && launchSound != null)
        {
            audioSource.PlayOneShot(launchSound);
        }

        // Find the active win condition
        winCondition = FindObjectsOfType<MonoBehaviour>().OfType<IWinCondition>().FirstOrDefault();

        if (winCondition == null)
        {
            Debug.LogWarning("No IWinCondition found in the scene — hits won't affect win/loss state.");
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

        // Rotate visual to match movement direction
        if (visualChild != null)
        {
            float angle = Vector2.SignedAngle(Vector2.down, direction);
            visualChild.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (audioSource != null && hitSound != null &&
            (other.CompareTag("Player") || other.CompareTag("Enemy")))
        {
            AudioSource.PlayClipAtPoint(hitSound, transform.position);
        }

        // Register hit with win condition if relevant
        if (winCondition != null && other.CompareTag("Player"))
        {
            // Instead of knowing exact win condition type, we call a standard method
            if (winCondition is IProjectileReactive projectileReactive)
            {
                projectileReactive.OnProjectileHit(other.tag);
            }
        }

        // Destroy projectile on impact
        if (other.CompareTag("Player") || other.CompareTag("Enemy") || other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
