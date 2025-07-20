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

    private Rigidbody2D rb;
    private player player;
    private Vector2 moveDir;
    private bool isMoving = false;
    private bool gamedone = false;

    public bool win = false;
    public bool lose = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player").GetComponent<player>();
    }

    void FixedUpdate()
    {
        dist.text = "x : " + ((int)(epw2.position.x - transform.position.x)) +
                    " y : " + ((int)(epw2.position.y - transform.position.y + 0.5f));

        if (!isMoving && player.p2move && player.movesave.Count > 0)
        {
            StartCoroutine(HandleMovement());
        }

        if (player.movesave.Count == 0 && !isMoving && !win)
        {
            StartCoroutine(wait());
        }

        if (gamedone && !win)
        {
            Debug.Log("U lose");
            lose = true;
        }
    }

    IEnumerator HandleMovement()
    {
        isMoving = true;

        while (player.movesave.Count > 0)
        {
            float moveVal = player.movesave.Dequeue();
            if (Mathf.Abs(moveVal) == 1f)
                moveDir = new Vector2(moveVal, 0);
            else if (Mathf.Abs(moveVal) == 2f)
                moveDir = new Vector2(0, moveVal / 2);

            animator.SetFloat("horizontal", moveDir.x);
            animator.SetFloat("vertical", moveDir.y);
            animator.SetFloat("speed", moveDir.sqrMagnitude);

            Vector2 start = rb.position;
            Vector2 target = start + moveDir;

            // Try push before moving
            TryPush(moveDir);

            while (Vector2.Distance(rb.position, target) > 0.01f)
            {
                Vector2 newPos = Vector2.MoveTowards(rb.position, target, player.walk * Time.fixedDeltaTime);
                rb.MovePosition(newPos);
                yield return new WaitForFixedUpdate();
            }

            rb.position = target;
            rb.velocity = Vector2.zero;

            yield return new WaitForSeconds(0.3f); // brief pause after each move
        }

        isMoving = false;
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(0.5f);
        gamedone = true;
    }

    void TryPush(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, detectDistance, pushableLayer);
        if (hit.collider != null)
        {
            movableblocks pushable = hit.collider.GetComponent<movableblocks>();
            if (pushable != null)
                pushable.Push(direction);
        }
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
