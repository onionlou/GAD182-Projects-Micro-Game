using UnityEngine;

/// <summary>
/// Controls movement and collision of the fireball projectile.
/// Direction is set externally via SetDirection().
/// </summary>
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

        // Find the win condition in the scene
        winCondition = FindObjectOfType<MonoBehaviour>() as IWinCondition;
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

            // Register the hit with the win condition
            if (winCondition is DodgeWinCondition dodgeWin)
            {
                dodgeWin.RegisterHit(other.tag);
            }
            else if (winCondition is CannonFodderWinCondition cannonWin)
            {
                cannonWin.RegisterHit(other.tag);
            }

            Destroy(gameObject);
        }
        else if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}