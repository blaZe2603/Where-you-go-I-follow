using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class player : MonoBehaviour
{
    public float detectDistance = 1f;
    public Animator animator;
    public GameManager gameManager;
    public GameObject targetObject;
    public float walk;
    public bool movepossibility = true;
    public Queue<float> movesave = new Queue<float>();
    public bool p2move = false;
    public Transform epw1;
    public TextMeshProUGUI dist;
    public LayerMask obstacleMask;
    public LayerMask pushableLayer;
    public float moveStep = 1f;

    private Rigidbody2D rb;
    private Vector2 input;
    private bool isMoving = false;
    private Vector2 targetPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        movesave.Enqueue(0);
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        targetPosition = rb.position;
    }

    void FixedUpdate()
    {
        dist.text = "x : " + ((int)(epw1.transform.position.x - transform.position.x)) + 
                    " y : " + ((int)(epw1.transform.position.y - transform.position.y + 0.5f));

        if (!isMoving && movepossibility)
        {
            input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            if (Mathf.Abs(input.x) > 0 && input.y == 0)
                StartCoroutine(AttemptMove(new Vector2(input.x, 0), input.x, 1f));
            else if (Mathf.Abs(input.y) > 0 && input.x == 0)
                StartCoroutine(AttemptMove(new Vector2(0, input.y), input.y, 2f));
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

    IEnumerator AttemptMove(Vector2 direction, float moveValue, float value)
    {
        movepossibility = false;

        if (TryPush(direction) || CanMove(direction))
        {
            movesave.Enqueue(moveValue * value);
            targetPosition = rb.position + direction * moveStep;
            isMoving = true;
        }

        yield return new WaitForSeconds(0.3f);
        movepossibility = true;
    }

    bool TryPush(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, detectDistance, pushableLayer);
        if (hit.collider != null)
        {
            movableblocks pushable = hit.collider.GetComponent<movableblocks>();
            if (pushable != null)
                return pushable.Push(direction);
        }
        return false;
    }

    bool CanMove(Vector2 direction)
    {
        Vector2 targetPos = rb.position + direction;
        Collider2D obstacleHit = Physics2D.OverlapBox(targetPos, Vector2.one * 0.8f, 0f, obstacleMask);
        return obstacleHit == null;
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
            targetObject.GetComponent<player2>().lose = true;
        }
    }
}
