using UnityEngine;

public class BH_Player : MonoBehaviour
{
    Animator animator;
    Rigidbody2D RB;
    Vector3 MousePos;

    [Tooltip("adds deadzone to stop RB.AddForce from tweaking")]
    public float ThresholdRange = 0.5f;
    public float Speed = 15f;
    public bool IsDead;

    public System.Action OnPlayerHit; // notify BH_WinCondition

    void Start()
    {
        animator = GetComponent<Animator>();
        RB = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!IsDead)
        {
            MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            animator.SetFloat("Movement", RB.velocity.x);

            if (transform.position.x < MousePos.x + ThresholdRange)
                RB.AddForce(new Vector2(Speed, 0));

            if (transform.position.x > MousePos.x - ThresholdRange)
                RB.AddForce(new Vector2(-Speed, 0));
        }
    }

    public void PlayerHit()
    {
        if (IsDead) return;

        IsDead = true;
        RB.constraints = RigidbodyConstraints2D.None;
        RB.drag = 1;
        RB.AddForce(new Vector2(0, 1000));

        Debug.Log("BH_Player: player dead");
        OnPlayerHit?.Invoke(); // notify win condition
    }
}
