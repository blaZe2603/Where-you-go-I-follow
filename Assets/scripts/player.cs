using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class player : MonoBehaviour
{
    float detectDistance = 1f;
    [SerializeField] Animator animator;
    public GameManager gameManager;
    // public GameObject targetObject;
    public float walk = 5f;
    [SerializeField] Transform epw1;
    [SerializeField] TextMeshProUGUI dist;
    [SerializeField] LayerMask obstacleMask;
    [SerializeField] LayerMask pushableLayer;
    public float moveStep = 1f;

    public bool movepossibility = true;
    public Queue<float> movesave = new Queue<float>();
    public bool p2move = false;


    private Rigidbody2D rb;
    private Vector2 input;
    private Vector2 targetPosition;
    private bool isMoving = false;
    private player2 p_2;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // p_2 = targetObject.GetComponent<player2>();
        targetPosition = rb.position;
    }

    void FixedUpdate()
    {
        dist.text = $"x : {(int)(epw1.position.x - targetPosition.x)} y : {(int)(epw1.position.y - targetPosition.y + 0.5f)}";

        if (!isMoving && movepossibility)
        {
            input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if (Mathf.Abs(input.x) > 0) input.y = 0; // No diagonal movement

            if (input != Vector2.zero)
                StartCoroutine(AttemptMove(input));
        }

        if (isMoving)
        {
            Vector2 newPos = Vector2.MoveTowards(rb.position, targetPosition, walk * Time.fixedDeltaTime);
            rb.MovePosition(newPos);

            if (Vector2.Distance(rb.position, targetPosition) < 0.01f)
            {
                rb.position = targetPosition;
                rb.velocity = Vector2.zero;
                isMoving = false;
            }
        }

        animator.SetFloat("horizontal", input.x);
        animator.SetFloat("vertical", input.y);
        animator.SetFloat("speed", input.sqrMagnitude);

        if (Input.GetKeyDown(KeyCode.Z))
            gameManager.gameover();
    }

    IEnumerator AttemptMove(Vector2 direction)
    {
        movepossibility = false;

        if (!CanMove(direction))
        {
            movepossibility = true;
            yield break;
        }

        TryPush(direction);

        float moveVal = direction.x != 0 ? direction.x : direction.y * 2;
        movesave.Enqueue(moveVal);
        
        targetPosition = rb.position + direction * moveStep;
        isMoving = true;

        yield return new WaitForSeconds(0.3f);
        movepossibility = true;
    }

    bool CanMove(Vector2 direction)
    {
        Vector2 boxSize = new Vector2(0.8f, 0.8f);
        Vector2 checkPos = rb.position + direction * moveStep;

        // If pushable is present, check if it can be pushed
        RaycastHit2D hit = Physics2D.Raycast(rb.position, direction, detectDistance, pushableLayer);
        if (hit.collider != null)
        {
            movableblocks block = hit.collider.GetComponent<movableblocks>();
            if (block != null)
                return block.CanBePushed(direction, obstacleMask);
        }

        Collider2D obstacle = Physics2D.OverlapBox(checkPos, boxSize, 0f, obstacleMask);
        return obstacle == null;
    }

    void TryPush(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(rb.position, direction, detectDistance, pushableLayer);
        if (hit.collider != null)
        {
            movableblocks block = hit.collider.GetComponent<movableblocks>();
            block?.Push(direction);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("end"))
        {
            gameManager.gameover();
            p2move = true;
        }

        if (collision.gameObject.CompareTag("spike"))
        {
            Destroy(gameObject);
            p_2.lose = true;
        }
    }
}
