using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttons : MonoBehaviour
{
    [SerializeField] private end_point endpoint;  // Reference to the central end_point manager

    private bool isPressed = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("movable") && !isPressed)
        {
            isPressed = true;
            endpoint.button_count++;
            Debug.Log("Button Pressed! Total Count: " + endpoint.button_count);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("movable") && isPressed)
        {
            isPressed = false;
            endpoint.button_count--;
            Debug.Log("Button Released! Total Count: " + endpoint.button_count);
        }
    }
}