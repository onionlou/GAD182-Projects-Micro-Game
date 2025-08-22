using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    [SerializeField] private bool horizontalOnly = false;

    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.Log("PlayerMovement: No Animator found — animation will be skipped.");
        }
    }

    void Update()
    {
        // Get raw input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (horizontalOnly)
        {
            movement.y = 0f; // Lock vertical movement
        }
        else
        {
            // Prioritize stronger axis for 4-directional locking
            if (Mathf.Abs(movement.x) > Mathf.Abs(movement.y))
                movement.y = 0f;
            else
                movement.x = 0f;
        }

        // Update animator if present
        if (animator != null)
        {
            animator.SetFloat("MoveX", movement.x);
            animator.SetFloat("MoveY", movement.y);
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}