using UnityEngine;

public class movableblocks : MonoBehaviour
{
    public float pushDistance = 1f;
    public float pushSpeed = 5f;
    public LayerMask obstacleMask;

    private Rigidbody2D rb;
    private Vector2 targetPosition;
    private bool isMoving = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        targetPosition = rb.position;
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            rb.MovePosition(Vector2.MoveTowards(rb.position, targetPosition, pushSpeed * Time.fixedDeltaTime));

            if (Vector2.Distance(rb.position, targetPosition) < 0.01f)
            {
                rb.position = targetPosition; // Snap
                isMoving = false;
                rb.velocity = Vector2.zero;
            }
        }
    }

    public void Push(Vector2 direction)
    {
        if (isMoving)
            return;

        Vector2 nextPos = rb.position + direction * pushDistance;

        // Raycast to check if the target position is blocked
        RaycastHit2D hit = Physics2D.Raycast(rb.position, direction, pushDistance, obstacleMask);
        if (hit.collider != null)
        {
            Debug.Log("Blocked by: " + hit.collider.name);
            return; // Don't move if there's an obstacle
        }

        targetPosition = nextPos;
        isMoving = true;
    }
}
