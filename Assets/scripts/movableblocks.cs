using Unity.VisualScripting;
using UnityEngine;

public class movableblocks : MonoBehaviour
{
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
        Vector2 checkPos = rb.position + direction * moveStep;
        Collider2D hit = Physics2D.OverlapBox(checkPos, new Vector2(0.8f, 0.8f), 0f, obstacleMask);
        return hit == null;
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
