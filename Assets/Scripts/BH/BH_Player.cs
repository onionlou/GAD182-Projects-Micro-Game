using UnityEngine;

public class BH_Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public int health = 3;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private BH_WinCondition winCondition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        winCondition = FindObjectOfType<BH_WinCondition>();

        if (rb == null)
            Debug.LogError("[BH] No Rigidbody2D found on Player!");
        if (winCondition == null)
            Debug.LogError("[BH] No BH_WinCondition found!");
    }

    private void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(moveX, moveY).normalized;
    }

    private void FixedUpdate()
    {
        if (rb != null)
            rb.velocity = moveInput * moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyBullet"))
        {
            health--;
            Debug.Log($"[BH_Player] Hit by bullet. Health now: {health}");

            if (winCondition != null)
            {
                Debug.Log("[BH_Player] Notifying WinCondition of hit.");
                winCondition.PlayerHit();
            }

            if (health <= 0 && winCondition != null)
            {
                Debug.Log("[BH_Player] Health <= 0. Triggering lose condition.");
                winCondition.TriggerLose();
                Destroy(gameObject);
            }

            Destroy(collision.gameObject);
        }
    }
}