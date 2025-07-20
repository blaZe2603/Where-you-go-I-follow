using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class player2 : MonoBehaviour
{
    public float detectDistance = 1f;
    public Animator animator;
    public Transform epw2;
    public TextMeshProUGUI dist;
    public LayerMask pushableLayer;
    public LayerMask obstacleMask;
    public float walk = 5f;
    public float moveStep = 1f;

    private Rigidbody2D rb;
    private Vector2 moveDir;
    private Vector2 targetPosition;
    private player player;
    private bool isMoving = false;
    private bool gamedone = false;

    public bool win = false;
    public bool lose = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player").GetComponent<player>();
        targetPosition = rb.position;
    }

    void FixedUpdate()
    {
        dist.text = $"x : {(int)(epw2.position.x - transform.position.x)} y : {(int)(epw2.position.y - transform.position.y + 0.5f)}";

        if (!isMoving && player.p2move && player.movesave.Count > 0)
        {
            float moveVal = player.movesave.Dequeue();
            moveDir = Mathf.Abs(moveVal) == 1 ? new Vector2(moveVal, 0) : new Vector2(0, moveVal / 2);

            if (CanMove(moveDir))
            {
                TryPush(moveDir);
                targetPosition = rb.position + moveDir * moveStep;
                isMoving = true;

                animator.SetFloat("horizontal", moveDir.x);
                animator.SetFloat("vertical", moveDir.y);
                animator.SetFloat("speed", moveDir.sqrMagnitude);
            }

            player.p2move = false; // Wait until player sets it again
        }

        if (isMoving)
        {
            Vector2 newPos = Vector2.MoveTowards(rb.position, targetPosition, walk * Time.fixedDeltaTime);
            rb.MovePosition(newPos);

            if (Vector2.Distance(rb.position, targetPosition) < 0.01f)
            {
                rb.position = targetPosition;
                isMoving = false;
                moveDir = Vector2.zero;
            }
        }

        if (player.movesave.Count == 0 && !isMoving && !win)
        {
            StartCoroutine(WaitAndLose());
        }

        if (gamedone && !win)
        {
            Debug.Log("U lose");
            lose = true;
        }
    }

    bool CanMove(Vector2 direction)
    {
        Vector2 checkPos = rb.position + direction * moveStep;

        // Check for obstacle at destination
        Collider2D obstacle = Physics2D.OverlapBox(checkPos, new Vector2(0.8f, 0.8f), 0f, obstacleMask);
        if (obstacle != null)
            return false;

        // If there's a pushable, check if it can move
        RaycastHit2D hit = Physics2D.Raycast(rb.position, direction, detectDistance, pushableLayer);
        if (hit.collider != null)
        {
            movableblocks block = hit.collider.GetComponent<movableblocks>();
            return block != null && block.CanBePushed(direction, obstacleMask);
        }

        return true;
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

    IEnumerator WaitAndLose()
    {
        yield return new WaitForSeconds(0.5f);
        gamedone = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("endw2"))
        {
            Debug.Log("U win");
            Destroy(collision.gameObject);
            gameObject.SetActive(false);
            win = true;
        }
        else if (collision.gameObject.CompareTag("spike"))
        {
            Debug.Log("U lose");
            gameObject.SetActive(false);
            lose = true;
        }
    }
}
