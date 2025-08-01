using Unity.VisualScripting;
using UnityEngine;

public class movableblocks : MonoBehaviour
{
    Vector2 lastDebugDir;
    public float pushSpeed = 5f;
    public float moveStep = 1f;

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
            Vector2 newPos = Vector2.MoveTowards(rb.position, targetPosition, pushSpeed * Time.fixedDeltaTime);
            rb.MovePosition(newPos);

            if (Vector2.Distance(rb.position, targetPosition) < 0.01f)
            {
                rb.position = targetPosition;
                rb.velocity = Vector2.zero;
                isMoving = false;
            }
        }
    }

    public bool Push(Vector2 direction)
    {
        if (isMoving) return false;

        Vector2 destination = rb.position + direction * moveStep;
        targetPosition = destination;
        isMoving = true;
        return true;
    }

    public bool CanBePushed(Vector2 direction, LayerMask obstacleMask)
    {
        // Store last direction for gizmo debug
        lastDebugDir = direction;

        // Calculate the target check position
        Vector2 checkPos = rb.position + direction * moveStep + new Vector2(0f,0.5f);



        // Check for obstacles at that position
        Collider2D[] hits = Physics2D.OverlapBoxAll(checkPos, new Vector2(0.4f, 0.4f), 0f, obstacleMask);

        foreach (var hit in hits)
        {
            if (hit.gameObject != gameObject)
            {
                Debug.Log($"Blocked by: {hit.name}");
                return false;
            }
        }

        return true;
    }
    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.red;

            Vector2 checkPos = rb.position + lastDebugDir * moveStep;

            // Match exactly with the detection size
            Gizmos.DrawWireCube(checkPos, new Vector3(0.4f, 0.4f, 0f));
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("spike"))
        {
            GetComponent<BoxCollider2D>().enabled = false;
            Destroy(collision.gameObject);
        }
    }
}
