using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    private player2 p2script;

    private void Start()
    {
        GameObject p2Object = GameObject.FindGameObjectWithTag("player2");
        p2script = p2Object.GetComponent<player2>();
    }

private void OnTriggerEnter2D(Collider2D collision)
{
    Debug.Log("Spike triggered by: " + collision.gameObject.name);

    if (collision.CompareTag("Player") || collision.CompareTag("player2"))
    {
        collision.gameObject.SetActive(false);
        p2script.lose = true;
    }
}
}

